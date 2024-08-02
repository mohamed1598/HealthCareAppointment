using Microsoft.EntityFrameworkCore;
using Patient.Application.Patient.Commands.RegisterPatient;
using Patient.Domain.Abstractions;
using Patient.Infrastructure.BackgroundServices;
using Patient.Infrastructure.Services;
using Patient.Persistence.DataSource;
using Patient.Persistence.Interceptors;
using Patient.Persistence.Repositories;
using Quartz;
using Shared.Helpers;
using Shared.Primitives;
using Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var memberConnectionString = builder.Configuration.GetConnectionString("PatientConnection");
builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptors>();
builder.Services.AddDbContext<PatientDbContext>((sp, optionsBuilder) =>
    {
        var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptors>()!;
        
        optionsBuilder
            .UseSqlServer(memberConnectionString)
            .AddInterceptors(interceptor);
    });
builder.Services.AddTransient<IEventBus, RabbitMQBus>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterPatientCommand).Assembly));
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(
            trigger =>
                trigger.ForJob(jobKey)
                .WithSimpleSchedule(
                    schedule =>
                        schedule.WithIntervalInSeconds(10)
                        .RepeatForever()
                )
        );
});
builder.Services.AddQuartzHostedService();
builder.Services.AddTransient<HandlerChecker>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

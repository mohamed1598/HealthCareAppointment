using Doctor.Application.Behaviors;
using Doctor.Application.Doctor.Commands.RegisterDoctor;
using Doctor.Domain.Abstractions;
using Doctor.Infrastructure.Services;
using Doctor.Persistence.DataSource;
using Doctor.Persistence.Interceptors;
using Doctor.Persistence.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Primitives;
using Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var patientConnectionString = builder.Configuration.GetConnectionString("DoctorConnection");
builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptors>();
builder.Services.AddDbContext<DoctorDbContext>((sp, optionsBuilder) =>
{
    var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptors>()!;

    optionsBuilder
        .UseSqlServer(patientConnectionString)
        .AddInterceptors(interceptor);
});
builder.Services.AddTransient<IEventBus, RabbitMQBus>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterDoctorCommand).Assembly));
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>),typeof(ValidationPipelineBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(RegisterDoctorCommand).Assembly, includeInternalTypes: true);
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

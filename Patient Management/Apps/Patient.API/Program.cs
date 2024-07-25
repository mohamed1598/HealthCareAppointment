using Microsoft.EntityFrameworkCore;
using Patient.Application.Patient.Commands.RegisterPatient;
using Patient.Application.Services;
using Patient.Domain.Abstractions;
using Patient.Infrastructure.DataSource;
using Patient.Infrastructure.Repositories;
using Shared.Primitives;
using Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var memberConnectionString = builder.Configuration.GetConnectionString("PatientConnection");
builder.Services.AddDbContext<PatientDbContext>(x => x.UseSqlServer(memberConnectionString));
builder.Services.AddTransient<IEventBus, RabbitMQBus>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterPatientCommand).Assembly));
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

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

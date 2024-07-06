using Microsoft.EntityFrameworkCore;
using Patient.Infrastructure.DataSource;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var memberConnectionString = builder.Configuration.GetConnectionString("PatientConnection");
builder.Services.AddDbContext<PatientDbContext>(x => x.UseSqlServer(memberConnectionString));

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

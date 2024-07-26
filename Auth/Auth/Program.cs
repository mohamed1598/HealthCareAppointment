using Auth.CommandHandlers;
using Auth.Commands;
using Auth.Extensions;
using Auth.Infrastructure;
using Auth.Middlewares;
using Microsoft.EntityFrameworkCore;
using Shared.IntegrationEvents;
using Shared.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AuthDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly,typeof(AddUserToRoleCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient<IEventBus, RabbitMQBus>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await app.Seed();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<ProfileCreatedIntegrationEvent>();
eventBus.Subscribe<ProfileEmailUpdatedIntegrationEvent>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidatorMiddleware>(typeof(Program).Assembly);

app.MapControllers();

app.Run();

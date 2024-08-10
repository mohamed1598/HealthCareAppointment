using Appointments.API.Appointments.Commands.ScheduleAppointment;
using Appointments.API.EventStore;
using EventStore.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string connectionString = "esdb+discover://admin:changeit@localhost:2113?tls=true&tlsVerifyCert=false";

var settings = EventStoreClientSettings.Create(connectionString);

var client = new EventStoreClient(settings);

var store = new EsAggregateStore(client);
builder.Services.AddSingleton<IAggregateStore>(store);
//builder.Services.AddSingleton(client);


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ScheduleAppointmentCommand).Assembly));



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

using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Primitives;
using System.Text;

namespace Shared.RabbitMq;

public sealed class RabbitMQBus(IMediator mediator) : IEventBus
{
    private readonly IMediator _mediator = mediator;
    private readonly List<Type> _eventTypes = [];

    public void Publish<T>(T @event) where T : IRequest
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventName = @event.GetType().Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
    }

    public Task SendCommand<T>(T command) where T : IRequest
    {
        return _mediator.Send(command);
    }

    public void Subscribe<T>()
        where T : IRequest
    {
        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));
        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : IRequest
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            DispatchConsumersAsync = true,
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var eventName = typeof(T).Name;

        channel.QueueDeclare(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += Consumer_Received;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception) { }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        try
        {
            var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
            var @event = JsonConvert.DeserializeObject(message, eventType!);
            await _mediator.Send(@event!);
        }
        catch { }

    }
}

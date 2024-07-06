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
    private readonly Dictionary<string, List<Type>> _handlers = [];
    private readonly List<Type> _eventTypes = [];

    public void Publish<T>(T @event) where T : IDomainEvent
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

    public void Subscribe<T, TH>()
        where T : IDomainEvent
        where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
            _eventTypes.Add(typeof(T));
        if (!_handlers.ContainsKey(eventName))
            _handlers.Add(eventName, []);
        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            throw new ArgumentException($"Handler type {handlerType.Name} already is registered for '{eventName}'");
        _handlers[eventName].Add(handlerType);
        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : IDomainEvent
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
        if (_handlers.TryGetValue(eventName, out List<Type>? subscriptions))
        {
            foreach ( var subscription in subscriptions )
            {
                var handler = Activator.CreateInstance(subscription);
                if (handler is null)
                    continue;
                var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                var @event = JsonConvert.DeserializeObject(message, eventType!);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType!);
                await (Task) concreteType.GetMethod("Handle")!.Invoke(handler,new object[] {@event!})!;
            }
        }
    }
}

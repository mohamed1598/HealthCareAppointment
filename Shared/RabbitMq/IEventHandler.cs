
using Shared.Primitives;

namespace Shared.RabbitMq;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IDomainEvent
{
    Task Handle(TEvent @event);
}

public interface IEventHandler
{
}
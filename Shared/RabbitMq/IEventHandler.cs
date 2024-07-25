
using MediatR;
using Shared.Primitives;

namespace Shared.RabbitMq;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IDomainEvent
{
}

public interface IEventHandler : IRequestHandler<IRequest>
{
}
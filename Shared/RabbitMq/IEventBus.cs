using MediatR;
using Shared.Primitives;

namespace Shared.RabbitMq;

public interface IEventBus
{
    Task SendCommand<T>(T command) where T : IRequest;
    void Publish<T>(T @event) where T : IDomainEvent;
    void Subscribe<T, TH>() where T : IDomainEvent
        where TH : IEventHandler<T>;

}

using MediatR;
using Shared.Primitives;

namespace Shared.RabbitMq;

public interface IEventBus
{
    Task SendCommand<T>(T command) where T : IRequest;
    void Publish<T>(T @event) where T : IRequest;
    void Subscribe<T>() where T : IRequest;

}

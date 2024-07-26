using Auth.Commands;
using MediatR;
using Shared.IntegrationEvents;

namespace Auth.IntegrationEventHandlers;

public class ProfileEmailUpdatedIntegrationEventHandler(ISender _sender) : IRequestHandler<ProfileEmailUpdatedIntegrationEvent>
{
    public async Task Handle(ProfileEmailUpdatedIntegrationEvent request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateUserEmailCommand(request.Email, request.NewEmail));
    }
}


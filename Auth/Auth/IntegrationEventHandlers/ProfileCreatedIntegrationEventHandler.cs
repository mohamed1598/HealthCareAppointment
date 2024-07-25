using Auth.Commands;
using MediatR;

namespace Auth.IntegrationEventHandlers;

public class ProfileCreatedIntegrationEventHandler(ISender _sender) : IRequestHandler<ProfileCreatedIntegrationEvent>
{
    public async Task Handle(ProfileCreatedIntegrationEvent request, CancellationToken cancellationToken)
    {
        await _sender.Send(new AddUserToRoleCommand(request.UserId, request.RoleName), cancellationToken);
    }
}

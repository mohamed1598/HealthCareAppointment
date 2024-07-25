using Auth.Commands;
using Auth.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.RabbitMq;
using Shared.Result;

namespace Auth.CommandHandlers;

public class AddUserToRoleCommandHandler(IServiceScopeFactory scope) : IRequestHandler<AddUserToRoleCommand>
{

    public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        using UserManager<IdentityUser> _userManager = scope.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
         
        var user = await  _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return; 

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
    }

}

using Auth.Commands;
using Auth.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.CommandHandlers;

public class RegisterUserCommandHandler(UserManager<IdentityUser> userManager) : IRequestHandler<RegisterUserCommand, IdentityResult>
{
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded) return result;
        await _userManager.AddToRoleAsync(user, AppRoles.NoProfileUser);
        return result;
    }
}

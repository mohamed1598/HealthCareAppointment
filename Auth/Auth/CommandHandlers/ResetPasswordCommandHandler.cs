using Auth.Commands;
using Auth.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.CommandHandlers;

public class ResetPasswordCommandHandler(UserManager<IdentityUser> userManager) : IRequestHandler<ResetPasswordCommand, IdentityResult>
{
    private readonly UserManager<IdentityUser> _userManager = userManager;

    public async Task<IdentityResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
    
        return await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
    }
}
using Auth.Commands;
using Auth.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.RabbitMq;
using Shared.Result;

namespace Auth.CommandHandlers;

public class UpdateUserEmailCommandHandler(IServiceScopeFactory scope) : IRequestHandler<UpdateUserEmailCommand>
{

    public async Task Handle(UpdateUserEmailCommand request, CancellationToken cancellationToken)
    {
        using UserManager<IdentityUser> _userManager = scope.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return;
        user.Email = request.NewEmail;
        user.UserName = request.NewEmail;
        user.NormalizedEmail = _userManager.NormalizeEmail(request.NewEmail);
        user.NormalizedUserName = _userManager.NormalizeName(request.NewEmail);

        var result = await _userManager.UpdateAsync(user);
    }

}

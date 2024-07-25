using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Auth.Queries.IsUserIdValid;

public class GetUserIdByEmailQueryHandler(UserManager<IdentityUser> userManager) : IRequestHandler<GetUserIdByEmailQuery, string>
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    public async Task<string> Handle(GetUserIdByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return string.Empty;
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
            return string.Empty;
        return user.Id;
    }
}

using Auth.Commands;
using Auth.Dtos;
using Auth.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.CommandHandlers;

public class LoginCommandHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, JwtHelper jwtHelper) : IRequestHandler<LoginCommand, LoginResponse?>
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly JwtHelper _jwtHelper = jwtHelper;

    public async Task<LoginResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var roles = await _userManager.GetRolesAsync(user!);
            var token = _jwtHelper.CreateToken(user!,roles.ToArray());
            return new LoginResponse(request.Email,token,roles.ToArray());
        }

        return null;
    }
}
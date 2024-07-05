using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Commands;

public class RegisterUserCommand(string email, string password) : IRequest<IdentityResult>
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
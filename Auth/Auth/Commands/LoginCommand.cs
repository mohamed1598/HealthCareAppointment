using Auth.Dtos;
using MediatR;

namespace Auth.Commands;

public class LoginCommand(string email, string password) : IRequest<LoginResponse?>
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
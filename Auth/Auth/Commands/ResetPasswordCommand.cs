using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Commands;

public class ResetPasswordCommand(string email, string currentPassword, string newPassword) : IRequest<IdentityResult>
{
    public string Email { get; set; } = email;
    public string NewPassword { get; set; } = newPassword;
    public string CurrentPassword { get; set; } = currentPassword;
}

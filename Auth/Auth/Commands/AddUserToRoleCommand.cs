using MediatR;

namespace Auth.Commands;

public record AddUserToRoleCommand(string UserId, string RoleName) : IRequest;

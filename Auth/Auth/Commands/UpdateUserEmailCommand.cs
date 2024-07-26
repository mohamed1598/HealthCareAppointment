using MediatR;

namespace Auth.Commands;

public record UpdateUserEmailCommand(string Email, string NewEmail):IRequest;
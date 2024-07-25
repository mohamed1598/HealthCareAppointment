using MediatR;
using Shared.Primitives;
using Shared.Result;

namespace Auth.Commands;

public record ProfileCreatedIntegrationEvent(string UserId, string RoleName):IRequest;
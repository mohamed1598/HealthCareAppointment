using MediatR;
using Shared.Result;

namespace Auth.Queries.IsUserIdValid;

public record GetUserIdByEmailQuery(string Email):IRequest<string>;

using Shared.Result;

namespace Doctor.Domain.Abstractions;

public interface IUserService
{
    Task<Result<string>> GetUserId(string userId);
}

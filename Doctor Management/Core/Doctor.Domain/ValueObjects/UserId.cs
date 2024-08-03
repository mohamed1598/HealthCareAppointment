using Doctor.Domain.Abstractions;
using Doctor.Domain.Errors;
using Shared.Primitives;
using Shared.Result;

namespace Doctor.Domain.ValueObjects;

public record UserId : ValueObject
{
    public UserId(string value) => Value = value;
    public string Value { get; }

    public async static Task<Result<UserId>> Create(string email,IUserService userService)
    {
        var userIdResult = await userService.GetUserId(email);
        if (userIdResult.IsFailure)
            return Result.Failure<UserId>(ValueObjectErrors.UserId.NotValid);
        return new UserId(userIdResult.Value!);
    }
}

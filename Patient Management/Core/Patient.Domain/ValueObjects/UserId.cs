using Patient.Domain.Abstractions;
using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record UserId : ValueObject
{
    public UserId(string value) => Value = value;
    public string Value { get; }

    public async static Task<Result<UserId>> Create(string email,IUserService userService)
    {
        if (string.IsNullOrEmpty(email))
            Result.Failure<UserId>(ValueObjectErrors.UserId.NotValid);

        var userIdResult = await userService.GetUserId(email);
        if (userIdResult.IsFailure)
            return Result.Failure<UserId>(ValueObjectErrors.UserId.NotValid);
        return new UserId(userIdResult.Value!);
    }
}

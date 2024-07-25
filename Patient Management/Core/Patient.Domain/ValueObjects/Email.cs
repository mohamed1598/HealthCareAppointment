using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public partial record Email : ValueObject
{
    private static readonly Regex ValidEmailRegex = EmailRegex();

    public Email(string value) => Value = value;
    public string Value { get; }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>(ValueObjectErrors.Email.Empty);

        if (!ValidEmailRegex.IsMatch(email))
            return Result.Failure<Email>(ValueObjectErrors.Email.InvalidFormat);

        return new Email(email);
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();
}
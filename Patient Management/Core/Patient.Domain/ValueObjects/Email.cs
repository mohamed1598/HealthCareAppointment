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

public record Email : ValueObject
{
    private static readonly Regex ValidEmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

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
}
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

public record PhoneNumber : ValueObject
{
    public const int MaxLength = 11; // Typical length of an Egyptian phone number (excluding country code)
    private static readonly Regex ValidPhoneNumberRegex = new Regex(@"^(011|010|012|015)[0-9]{8}$", RegexOptions.Compiled);

    public PhoneNumber(string value) => Value = value;
    public string Value { get; }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result.Failure<PhoneNumber>(ValueObjectErrors.PhoneNumber.Empty);

        if (phoneNumber.Length != MaxLength)
            return Result.Failure<PhoneNumber>(ValueObjectErrors.PhoneNumber.InvalidLength);

        if (!ValidPhoneNumberRegex.IsMatch(phoneNumber))
            return Result.Failure<PhoneNumber>(ValueObjectErrors.PhoneNumber.InvalidFormat);

        return new PhoneNumber(phoneNumber);
    }
}
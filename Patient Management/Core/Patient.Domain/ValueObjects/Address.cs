using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record Address : ValueObject
{
    public const int MaxLength = 200;

    public Address(string value) => Value = value;
    public string Value { get; }

    public static Result<Address> Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return Result.Failure<Address>(ValueObjectErrors.Address.Empty);

        if (address.Length > MaxLength)
            return Result.Failure<Address>(ValueObjectErrors.Address.TooLong);

        return new Address(address);
    }
}

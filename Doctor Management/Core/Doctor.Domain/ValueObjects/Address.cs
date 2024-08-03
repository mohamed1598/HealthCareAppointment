using Shared.Primitives;
using Shared.Result;

namespace Doctor.Domain.ValueObjects;

public record Address : ValueObject
{
    public Address(string value) => Value = value;
    public string Value { get; }

    public static Address Create(string address)
    {
        return new Address(address);
    }
}

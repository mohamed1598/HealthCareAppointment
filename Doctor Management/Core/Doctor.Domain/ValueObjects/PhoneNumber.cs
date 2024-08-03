using Shared.Primitives;
using Shared.Result;

namespace Doctor.Domain.ValueObjects;

public record PhoneNumber : ValueObject
{
    public PhoneNumber(string value) => Value = value;
    public string Value { get; }

    public static PhoneNumber Create(string phoneNumber)
    {
        return new PhoneNumber(phoneNumber);
    }
}
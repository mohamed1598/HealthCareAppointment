using Shared.Primitives;
using Shared.Result;

namespace Doctor.Domain.ValueObjects;

public partial record Email : ValueObject
{
    public Email(string value) => Value = value;
    public string Value { get; }

    public static Email Create(string email)
    {
        return new Email(email);
    }
}
using Shared.Primitives;
using Shared.Result;

namespace Doctor.Domain.ValueObjects;

public record Name : ValueObject
{
    public Name(string value) => Value = value;
    public string Value { get; }

    public static Name Create(string Name)
    {
        return new Name(Name);
    }
}

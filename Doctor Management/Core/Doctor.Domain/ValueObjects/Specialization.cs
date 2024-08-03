using Shared.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.ValueObjects;

public record Specialization : ValueObject
{
    public Specialization(string value) => Value = value;
    public string Value { get; }

    public static Specialization Create(string Name)
    {
        return new Specialization(Name);
    }
}
using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record Diagnosis : ValueObject
{
    public const int MaxLength = 200;

    public Diagnosis(string value) => Value = value;
    public string Value { get; }

    public static Result<Diagnosis> Create(string diagnosis)
    {
        if (string.IsNullOrWhiteSpace(diagnosis))
            return Result.Failure<Diagnosis>(ValueObjectErrors.Diagnosis.Empty);

        if (diagnosis.Length > MaxLength)
            return Result.Failure<Diagnosis>(ValueObjectErrors.Diagnosis.TooLong);

        return new Diagnosis(diagnosis);
    }
}

using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record Treatment : ValueObject
{
    public const int MaxLength = 500;

    public Treatment(string value) => Value = value;
    public string Value { get; }

    public static Result<Treatment> Create(string treatment)
    {
        if (string.IsNullOrWhiteSpace(treatment))
            return Result.Failure<Treatment>(ValueObjectErrors.Treatment.Empty);

        if (treatment.Length > MaxLength)
            return Result.Failure<Treatment>(ValueObjectErrors.Treatment.TooLong);

        return new Treatment(treatment);
    }
}
using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record UserId : ValueObject
{
    public UserId(string value) => Value = value;
    public String Value { get; }

    public static Result<UserId> Create(string id)
    {
        if (string.IsNullOrEmpty(id))
            Result.Failure<PatientId>(ValueObjectErrors.PatientId.NotValid);

        return new UserId(id);
    }
}

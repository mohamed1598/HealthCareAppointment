using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects;

public record MedicalHistoryId : ValueObject
{
    public MedicalHistoryId(Guid value) => Value = value;
    public Guid Value { get; }

    public static Result<MedicalHistoryId> Create(Guid id)
    {
        if (id == default)
            Result.Failure<MedicalHistoryId>(ValueObjectErrors.MedicalHistoryId.NotValid);

        return new MedicalHistoryId(id);
    }
}
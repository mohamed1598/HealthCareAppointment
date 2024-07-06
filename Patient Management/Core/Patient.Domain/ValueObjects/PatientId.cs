using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Patient.Domain.ValueObjects;

public record PatientId : ValueObject
{
    public PatientId(Guid value) => Value = value;
    public Guid Value { get; }

    public static Result<PatientId> Create(Guid id)
    {
        if (id == default)
             Result.Failure<PatientId>(ValueObjectErrors.PatientId.NotValid);

        return new PatientId(id);
    }
}

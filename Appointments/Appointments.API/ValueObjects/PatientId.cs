using Shared.Primitives;

namespace Appointments.API.ValueObjects;

public record PatientId : ValueObject
{
    public PatientId(Guid value) => Value = value;
    public Guid Value { get; }

    public static PatientId Create(Guid id)
    {
        return new PatientId(id);
    }
}

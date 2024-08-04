using Shared.Primitives;

namespace Appointments.API.ValueObjects;

public record DoctorId : ValueObject
{
    public DoctorId(Guid value) => Value = value;
    public Guid Value { get; }

    public static DoctorId Create(Guid id)
    {
        return new DoctorId(id);
    }
}

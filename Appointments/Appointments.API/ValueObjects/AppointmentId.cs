using Shared.Primitives;

namespace Appointments.API.ValueObjects;

public record AppointmentId : ValueObject
{
    public AppointmentId(Guid value) => Value = value;
    public Guid Value { get; }

    public static AppointmentId Create(Guid id)
    {
        return new AppointmentId(id);
    }
}

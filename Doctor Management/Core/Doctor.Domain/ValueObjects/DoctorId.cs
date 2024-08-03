using Shared.Primitives;
using Shared.Result;
namespace Doctor.Domain.ValueObjects;

public record DoctorId : ValueObject
{
    public DoctorId(Guid value) => Value = value;
    public Guid Value { get; }

    public static DoctorId Create(Guid id)
    {
        return new DoctorId(id);
    }
}

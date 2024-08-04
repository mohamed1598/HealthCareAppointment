using Appointments.API.ValueObjects;
using Shared.Primitives;

namespace Appointments.API.DomainEvents;

public record AppointmentScheduledDomainEvent(
    AppointmentId AppointmentId,
    DoctorId DoctorId,
    PatientId PatientId,
    DateTime AppointmentDate) : IDomainEvent;

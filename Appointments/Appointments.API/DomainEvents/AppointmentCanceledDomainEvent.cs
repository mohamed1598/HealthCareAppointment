using Appointments.API.ValueObjects;
using Shared.Primitives;

namespace Appointments.API.DomainEvents;

public record AppointmentCanceledDomainEvent(
    AppointmentId AppointmentId) : IDomainEvent;

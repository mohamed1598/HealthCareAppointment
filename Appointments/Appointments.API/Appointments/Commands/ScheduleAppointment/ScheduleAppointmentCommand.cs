using Appointments.API.Models;
using Appointments.API.ValueObjects;
using MediatR;
using Shared.Primitives;
using Shared.Result;

namespace Appointments.API.Appointments.Commands.ScheduleAppointment;

public record ScheduleAppointmentCommand(
    Guid DoctorId,
    Guid PatientId,
    DateTime AppointmentDate) : IRequest<Result<Appointment>>;

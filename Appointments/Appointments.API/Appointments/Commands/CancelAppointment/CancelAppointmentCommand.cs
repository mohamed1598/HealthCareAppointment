using Appointments.API.Models;
using Appointments.API.ValueObjects;
using MediatR;
using Shared.Primitives;
using Shared.Result;

namespace Appointments.API.Appointments.Commands.CancelAppointment;

public record CancelAppointmentDetailsCommand(
    Guid AppointmentId) : IRequest<Result<Models.Appointment>>;

using Appointments.API.Models;
using Appointments.API.ValueObjects;
using MediatR;
using Shared.Result;

namespace Appointments.API.Appointments.Queries.GetAppointmentDetails;

public record GetAppointmentDetailsQuery(Guid Id) : IRequest<Result<Appointment>>;
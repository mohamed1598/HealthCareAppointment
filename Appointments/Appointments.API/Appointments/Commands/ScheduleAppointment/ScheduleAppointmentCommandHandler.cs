using Appointments.API.EventStore;
using Appointments.API.ValueObjects;
using MediatR;
using Shared.Result;

namespace Appointments.API.Appointments.Commands.ScheduleAppointment;

public class ScheduleAppointmentCommandHandler(IAggregateStore store) : IRequestHandler<ScheduleAppointmentCommand, Result<Models.Appointment>>
{
    public async Task<Result<Models.Appointment>> Handle(ScheduleAppointmentCommand request, CancellationToken cancellationToken)
    {
        var doctorId = DoctorId.Create(request.DoctorId);
        var patientId = PatientId.Create(request.PatientId);

        var appointment = Models.Appointment.Create(doctorId, patientId, request.AppointmentDate);

        await store.Save<Models.Appointment, AppointmentId>(appointment);

        return Result.Success<Models.Appointment>();
    }
}

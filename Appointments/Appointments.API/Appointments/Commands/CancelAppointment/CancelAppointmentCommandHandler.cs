using Appointments.API.EventStore;
using Appointments.API.Models;
using Appointments.API.ValueObjects;
using EventStore.Client;
using MediatR;
using Shared.Result;

namespace Appointments.API.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler(IAggregateStore store) : IRequestHandler<CancelAppointmentDetailsCommand, Result<Models.Appointment>>
{
    public async Task<Result<Models.Appointment>> Handle(CancelAppointmentDetailsCommand request, CancellationToken cancellationToken)
    {
        var appointmentId = AppointmentId.Create(request.AppointmentId);
        Models.Appointment appointment;
        try
        {
            appointment = await store.Load<Models.Appointment, AppointmentId>(appointmentId);
        }
        catch (StreamNotFoundException ex)
        {
            return Result.Failure<Models.Appointment>(new Error("AppointmentNotExist", "Appointment Doesn't Exist"));
        }


        appointment.Cancel();

        await store.Save<Models.Appointment, AppointmentId>(appointment);

        return Result.Success<Models.Appointment>();
    }
}

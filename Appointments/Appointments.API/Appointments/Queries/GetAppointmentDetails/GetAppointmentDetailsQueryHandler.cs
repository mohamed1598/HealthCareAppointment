using Appointments.API.EventStore;
using Appointments.API.Models;
using Appointments.API.ValueObjects;
using EventStore.Client;
using MediatR;
using Shared.Result;

namespace Appointments.API.Appointments.Queries.GetAppointmentDetails;

public class GetAppointmentDetailsQueryHandler(IAggregateStore store) : IRequestHandler<GetAppointmentDetailsQuery, Result<Appointment>>
{
    public async Task<Result<Appointment>> Handle(GetAppointmentDetailsQuery request, CancellationToken cancellationToken)
    {
        var appointmentId = AppointmentId.Create(request.Id);
        Appointment appointment;
        try
        {
            appointment = await store.Load<Appointment, AppointmentId>(appointmentId);
        }
        catch (StreamNotFoundException _)
        {
            return Result.Failure<Appointment>(new Error("AppointmentNotExist", "Appointment Doesn't Exist"));
        }


        return appointment;
    }
}

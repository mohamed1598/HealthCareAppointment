using Appointments.API.DomainEvents;
using Appointments.API.ValueObjects;
using Shared.Primitives;

namespace Appointments.API.Models;

public class Appointment: AggregateRoot<AppointmentId>
{
    public AppointmentId Id { get; private set; }
    public DoctorId DoctorId { get; private set; }
    public PatientId PatientId { get; private set; }
    public DateTime AppointmentDate { get; private set; }
    public AppointmentStatus Status { get; private set; }

    protected Appointment(): base(AppointmentId.Create(Guid.NewGuid()))
    {

    }
    public void Schedule(DoctorId doctorId, PatientId patientId, DateTime appointmentDate)
    {
        if (Status == AppointmentStatus.Scheduled)
        {
            DoctorId = doctorId;
            PatientId = patientId;
            AppointmentDate = appointmentDate;

            RaiseDomainEvent(new AppointmentScheduledDomainEvent(Id,doctorId,patientId,appointmentDate));
        }
        else
        {
            throw new InvalidOperationException("Cannot schedule an appointment that is not in the scheduled state.");
        }
    }

    public void Cancel()
    {
        Status = AppointmentStatus.Canceled;

        RaiseDomainEvent(new AppointmentCanceledDomainEvent(Id));
    }

    public void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case AppointmentScheduledDomainEvent scheduledEvent:
                Id = scheduledEvent.AppointmentId;
                DoctorId = scheduledEvent.DoctorId;
                PatientId = scheduledEvent.PatientId;
                AppointmentDate = scheduledEvent.AppointmentDate;
                Status = AppointmentStatus.Scheduled;
                break;
            case AppointmentCanceledDomainEvent canceledEvent:
                Status = AppointmentStatus.Canceled;
                break;
        }

    }

    public static Appointment RebuildFromEvents(IEnumerable<IDomainEvent> events)
    {
        var appointment = new Appointment(); 

        foreach (var evt in events)
        {
            appointment.Apply(evt);
        }

        return appointment; 
    }
}

public enum AppointmentStatus
{
    Scheduled,   
    Canceled,    
    Completed,   
    Rescheduled   
}

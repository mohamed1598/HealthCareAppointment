using Doctor.Domain.DomainEvents;
using Doctor.Domain.ValueObjects;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Entities;

public class Doctor : AggregateRoot<DoctorId>
{
    protected Doctor(DoctorId id) : base(id)
    {
    }

    internal Doctor(DoctorId id, Name name, Availability availability, ContactDetails contactDetails, Specialization specialization, UserId userId) : base(id)
    {
        Name = name;
        Availability = availability;
        ContactDetails = contactDetails;
        Specialization = specialization;
        UserId = userId;
    }

    public Name Name { get; private set; }

    public Availability Availability { get; private set; }

    public ContactDetails ContactDetails { get; private set; }

    public Specialization Specialization { get; private set; }

    public UserId UserId { get; private set; }

    public static Result<Doctor> Create(Name name, Availability availability, ContactDetails contactDetails, Specialization specialization, UserId userId)
    {
        var doctorId = DoctorId.Create(Guid.NewGuid());

        Doctor doctor = new(doctorId, name, availability, contactDetails, specialization, userId);
        doctor.RaiseDomainEvent(new DoctorProfileCreatedDomainEvent(doctorId, name, availability, contactDetails, specialization, userId));

        return doctor;

    }
    public void Update(Name name, Availability availability, ContactDetails contactDetails, Specialization specialization)
    {
        Name = name;
        Availability = availability;
        ContactDetails = contactDetails;
        Specialization = specialization;

        RaiseDomainEvent(new DoctorProfileUpdatedDomainEvent(Id, Name, Availability, ContactDetails, Specialization));
    }
}


public enum Availability
{
    Available,
    Unavailable
}
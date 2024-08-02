using Patient.Domain.DomainEvents;
using Patient.Domain.Errors;
using Patient.Domain.ValueObjects;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.Entities;

public class Patient : AggregateRoot<PatientId>
{
    private readonly List<MedicalHistory> _medicalHistories = [];
    protected Patient() : base(PatientId.Create(Guid.NewGuid()).Value!) { }
    internal Patient(PatientId id , Name name, DateOfBirth dateOfBirth ,ContactDetails contactDetails,UserId userId) : base(id)
    {
        Name = name;
        ContactDetails = contactDetails;
        DateOfBirth = dateOfBirth;
        UserId = userId;
    }

    public Name Name { get; private set; }
    public DateOfBirth DateOfBirth { get; private set; }
    public ContactDetails ContactDetails { get; private set; }
    public UserId UserId { get; private set; }

    public IReadOnlyCollection<MedicalHistory> MedicalHistories => _medicalHistories;
    public static Result<Patient> Create(Name name, DateOfBirth dateOfBirth, ContactDetails contactDetails, UserId userId)
    {
        var patientIdResult = PatientId.Create(Guid.NewGuid());
        if (patientIdResult.IsFailure)
            return Result.Failure<Patient>(patientIdResult.Error!);
        
        Patient patient = new(patientIdResult.Value!, name, dateOfBirth, contactDetails, userId);
        patient.RaiseDomainEvent(new PatientProfileCreatedDomainEvent(patientIdResult.Value!, name, dateOfBirth, contactDetails, userId));

        return patient;

    }
    public void Update(Name name, DateOfBirth dateOfBirth, ContactDetails contactDetails)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        ContactDetails = contactDetails;

        RaiseDomainEvent(new PatientProfileUpdatedDomainEvent(Id, Name, DateOfBirth, ContactDetails));
    }

    public void AddMedicalHistoryRecord(MedicalHistory history)
    {
        _medicalHistories.Add(history);
        RaiseDomainEvent(new PatientMedicalHistoryRecordAddedDomainEvent(Id, history));
    }

    public Result<string> RemoveMedicalHistoryRecord(MedicalHistoryId id)
    {
        var record =_medicalHistories.FirstOrDefault(mh => mh.Id == id);
        if(record is null) return Result.Failure<string>(MedicalHistoryErrors.NotFound);

        record.IsDeleted = true;
        RaiseDomainEvent(new PatientMedicalHistoryRecordDeletedDomainEvent(Id, id));
        return Result.Success<string>("Medical History Removed Successfully");
    }
}


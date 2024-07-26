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
    protected Patient() : base(PatientId.Create(Guid.NewGuid()).Value) { }
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
            //log
            return Result.Failure<Patient>(patientIdResult.Error!);

        return new Patient(patientIdResult.Value, name, dateOfBirth, contactDetails, userId);

    }
    public void Update(Name name, DateOfBirth dateOfBirth, ContactDetails contactDetails)
    {
        Name = name;
        DateOfBirth = dateOfBirth;
        ContactDetails = contactDetails;
    }

    public void AddMedicalHistoryRecord(MedicalHistory history)
    {
        _medicalHistories.Add(history);
    }

    public Result<string> RemoveMedicalHistoryRecord(MedicalHistoryId id)
    {
        var record =_medicalHistories.FirstOrDefault(mh => mh.Id == id);
        if(record is null) return Result.Failure<string>(MedicalHistoryErrors.NotFound);

        record.IsDeleted = true;
        return Result.Success<string>("Medical History Removed Successfully");
    }
}

using MediatR;
using Shared.Result;
using Entities = Patient.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient.Domain.Abstractions;
using Patient.Domain.Entities;
using Patient.Domain.ValueObjects;
using Shared.RabbitMq;
using Auth.Commands;
using Patient.Domain.Errors;
using Shared.IntegrationEvents;

namespace Patient.Application.Patient.Commands.UpdatePatientProfile;

public class UpdatePatientProfileCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository, IEventBus bus) : IRequestHandler<UpdatePatientProfileCommand, Result<Entities.Patient>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPatientRepository _patientRepository = patientRepository;
    private readonly IEventBus _bus = bus;

    public async Task<Result<Entities.Patient>> Handle(UpdatePatientProfileCommand request, CancellationToken cancellationToken)
    {
        var patientId = PatientId.Create(request.PatientId);
        if (patientId.IsFailure) return Result.Failure<Entities.Patient>(patientId.Error!);

        var patient = _patientRepository.GetPatientById(patientId.Value!);
        if (patient is null) return Result.Failure<Entities.Patient>(PatientErrors.NotFound);

        var mailBeforeModify = patient.ContactDetails.Email;

        var nameResult = Name.Create(request.Name);
        var dateOfBirthResult = DateOfBirth.Create(request.DateOfBirth);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var emailResult = Email.Create(request.Email);
        var addressResult = Address.Create(request.Address);
        var combinedResult = Result.Combine<Entities.Patient>(nameResult, dateOfBirthResult, phoneNumberResult, emailResult, addressResult);

        if (combinedResult.IsFailure)
            return Result.Failure<Entities.Patient>(combinedResult.Error!);

        var contactDetailsResult = ContactDetails.Create(phoneNumberResult.Value!, emailResult.Value!, addressResult.Value!);

        patient.Update(nameResult.Value!, dateOfBirthResult.Value!, contactDetailsResult);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (patient.ContactDetails.Email != mailBeforeModify)
            _bus.Publish(new ProfileEmailUpdatedIntegrationEvent(mailBeforeModify.Value, patient.ContactDetails.Email.Value));

        return Result.Success<Entities.Patient>();
    }
    
}

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

namespace Patient.Application.Patient.Commands.RegisterPatient;

public class UpdatePatientProfileCommandHandler(IUnitOfWork unitOfWork, IPatientRepository patientRepository, IUserService userService, IEventBus bus) : IRequestHandler<RegisterPatientCommand, Result<Entities.Patient>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPatientRepository _patientRepository = patientRepository;
    private readonly IUserService _userService = userService;
    private readonly IEventBus _bus = bus;

    public async Task<Result<Entities.Patient>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(request.Name);
        var dateOfBirthResult = DateOfBirth.Create(request.DateOfBirth);
        var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
        var emailResult = Email.Create(request.Email);
        var addressResult = Address.Create(request.Address);
        var userIdResult = await UserId.Create(request.Email, _userService);
        var combinedResult = Result.Combine<Entities.Patient>(nameResult, dateOfBirthResult, phoneNumberResult, emailResult, addressResult, userIdResult);

        if (combinedResult.IsFailure)
            return Result.Failure<Entities.Patient>(combinedResult.Error!);

        var contactDetailsResult = ContactDetails.Create(phoneNumberResult.Value!, emailResult.Value!, addressResult.Value!);

        var patientResult = Entities.Patient.Create(nameResult.Value!, dateOfBirthResult.Value!, contactDetailsResult, userIdResult.Value!);
        if (patientResult.IsFailure)
            return Result.Failure<Entities.Patient>(patientResult.Error!);

        _patientRepository.Add(patientResult.Value!);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _bus.Publish(new ProfileCreatedIntegrationEvent(userIdResult.Value!.Value, "Patient"));

        return Result.Success<Entities.Patient>();
    }
}

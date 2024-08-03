using Doctor.Domain.Abstractions;
using Doctor.Domain.Entities;
using Doctor.Domain.Errors;
using Doctor.Domain.ValueObjects;
using MediatR;
using Shared.IntegrationEvents;
using Shared.RabbitMq;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Doctor.Commands.UpdateDoctor;

public class UpdateDoctorCommandHandler(IUnitOfWork unitOfWork, IDoctorRepository doctorRepository, IEventBus bus) : IRequestHandler<UpdateDoctorCommand, Result<Domain.Entities.Doctor>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IEventBus _bus = bus;

    public async Task<Result<Domain.Entities.Doctor>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorId = DoctorId.Create(request.DoctorId);

        var doctor = await _doctorRepository.GetDoctorDetails(doctorId);
        if (doctor is null) return Result.Failure<Domain.Entities.Doctor>(DoctorErrors.NotFound);

        var mailBeforeModify = doctor.ContactDetails.Email;

        var name = Name.Create(request.Name);
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        var email = Email.Create(request.Email);
        var address = Address.Create(request.Address);
        var availability = request.IsAvailable ? Availability.Available : Availability.Unavailable;
        var specialization = Specialization.Create(request.Specialization);
        var contactDetails = ContactDetails.Create(phoneNumber, email, address);

        doctor.Update(name, availability, contactDetails, specialization);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (doctor.ContactDetails.Email != mailBeforeModify)
            _bus.Publish(new ProfileEmailUpdatedIntegrationEvent(mailBeforeModify.Value, doctor.ContactDetails.Email.Value));

        return Result.Success<Domain.Entities.Doctor>();
    }
}

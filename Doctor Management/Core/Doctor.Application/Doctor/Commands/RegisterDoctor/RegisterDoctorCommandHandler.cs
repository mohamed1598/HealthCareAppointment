using Auth.Commands;
using Doctor.Domain.Abstractions;
using Doctor.Domain.Entities;
using Doctor.Domain.ValueObjects;
using MediatR;
using Shared.RabbitMq;
using Shared.Result;

namespace Doctor.Application.Doctor.Commands.RegisterDoctor;

public class RegisterDoctorCommandHandler(IUnitOfWork unitOfWork, IDoctorRepository doctorRepository, IUserService userService, IEventBus bus) : IRequestHandler<RegisterDoctorCommand, Result<Domain.Entities.Doctor>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IUserService _userService = userService;
    private readonly IEventBus _bus = bus;

    public async Task<Result<Domain.Entities.Doctor>> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        var name = Name.Create(request.Name);
        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        var email = Email.Create(request.Email);
        var address = Address.Create(request.Address);
        Availability availability = request.IsAvailable ? Availability.Available : Availability.Unavailable;
        var specialization = Specialization.Create(request.Specialization);

        var userIdResult = await UserId.Create(request.Email, _userService);
        if (userIdResult.IsFailure)
            return Result.Failure<Domain.Entities.Doctor>(userIdResult.Error!);

        var contactDetails = ContactDetails.Create(phoneNumber, email, address);

        var doctorResult = Domain.Entities.Doctor.Create(name, availability, contactDetails, specialization, userIdResult.Value!);
        if (doctorResult.IsFailure)
            return Result.Failure<Domain.Entities.Doctor>(doctorResult.Error!);

        _doctorRepository.Add(doctorResult.Value!);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _bus.Publish(new ProfileCreatedIntegrationEvent(userIdResult.Value!.Value, "Doctor"));

        return Result.Success<Domain.Entities.Doctor>();
    }
}

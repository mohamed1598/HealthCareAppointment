using MediatR;
using Shared.Result;

namespace Doctor.Application.Doctor.Commands.RegisterDoctor;

public sealed record RegisterDoctorCommand(string Name, bool IsAvailable, string PhoneNumber, string Address, string Email, string Specialization) : IRequest<Result<Domain.Entities.Doctor>>;
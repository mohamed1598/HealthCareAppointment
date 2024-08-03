using MediatR;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Doctor.Commands.UpdateDoctor;

public sealed record UpdateDoctorCommand(Guid DoctorId, string Name, bool IsAvailable, string PhoneNumber, string Address, string Email, string Specialization) : IRequest<Result<Domain.Entities.Doctor>>;
using MediatR;
using Shared.Result;
using Entities = Patient.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Commands.UpdatePatientProfile;

public sealed record UpdatePatientProfileCommand(Guid PatientId,string Name, DateTime DateOfBirth,string PhoneNumber, string Address ,string Email) : IRequest<Result<Entities.Patient>>;
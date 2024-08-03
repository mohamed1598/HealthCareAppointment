using Doctor.Domain.ValueObjects;
using MediatR;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Application.Doctor.Queries;

public record GetDoctorDetailsQuery(DoctorId Id) : IRequest<Result<Domain.Entities.Doctor>>;


using MediatR;
using Patient.Domain.Entities;
using Patient.Domain.ValueObjects;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Queries;

public record GetPatientDataQuery(PatientId Id):IRequest<Result<Domain.Entities.Patient>>;

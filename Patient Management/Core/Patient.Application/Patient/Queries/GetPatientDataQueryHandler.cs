using MediatR;
using Patient.Domain.Abstractions;
using Patient.Domain.Errors;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Queries;

public class GetPatientDataQueryHandler(IPatientRepository _patientRepository) : IRequestHandler<GetPatientDataQuery, Result<Domain.Entities.Patient>>
{
    public async Task<Result<Domain.Entities.Patient>> Handle(GetPatientDataQuery request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetPatientDetails(request.Id);
        if (patient is null) return Result.Failure<Domain.Entities.Patient>(PatientErrors.NotFound);
        return patient;
    }
}

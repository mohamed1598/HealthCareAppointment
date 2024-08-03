using Doctor.Domain.Abstractions;
using MediatR;
using Shared.Result;
using Entities = Doctor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctor.Domain.Errors;

namespace Doctor.Application.Doctor.Queries;

public class GetDoctorDetailsQueryHandler(IDoctorRepository _doctorRepository) : IRequestHandler<GetDoctorDetailsQuery, Result<Entities.Doctor>>
{
    public async Task<Result<Entities.Doctor>> Handle(GetDoctorDetailsQuery request, CancellationToken cancellationToken)
    {
        var patient = await _doctorRepository.GetDoctorDetails(request.Id);
        if (patient is null) return Result.Failure<Entities.Doctor>(DoctorErrors.NotFound);
        return patient;
    }
}

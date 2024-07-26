using MediatR;
using Patient.Domain.Abstractions;
using Patient.Domain.Entities;
using Patient.Domain.Errors;
using Patient.Domain.ValueObjects;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Commands.AddMedicalHistoryRecord;

public class AddMedicalHistoryRecordCommandHandler(IPatientRepository _patientRepository, IUnitOfWork _unitOfWork) : IRequestHandler<AddMedicalHistoryRecordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddMedicalHistoryRecordCommand request, CancellationToken cancellationToken)
    {
        var patientId = PatientId.Create(request.PatientId);
        if (patientId.IsFailure) return Result.Failure<string>(patientId.Error!);

        var patient = await _patientRepository.GetPatientData(patientId.Value!);
        if (patient is null) return Result.Failure<string>(PatientErrors.NotFound);

        var diagnosisResult = Diagnosis.Create(request.Diagnosis);
        if (diagnosisResult.IsFailure) return Result.Failure<string>(diagnosisResult.Error!);

        var treatmentResult = Treatment.Create(request.Treatment);
        var combinedResult = Result.Combine<string>(diagnosisResult, treatmentResult);

        if (combinedResult.IsFailure)
            return Result.Failure<string>(combinedResult.Error!);

        var medicalHistoryResult = MedicalHistory.Create(diagnosisResult.Value!,treatmentResult.Value!, request.Date?? DateTime.UtcNow, patientId.Value!);
        if(medicalHistoryResult.IsFailure)
            return Result.Failure<string>(medicalHistoryResult.Error!);

        patient.AddMedicalRecordHistory(medicalHistoryResult.Value!);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return "Medical History Record Added Successfully";
    }
}

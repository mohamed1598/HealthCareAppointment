using MediatR;
using Patient.Domain.Abstractions;
using Patient.Domain.Errors;
using Patient.Domain.ValueObjects;
using Shared.Result;

namespace Patient.Application.Patient.Commands.RemoveMedicalHistoryRecord;

public record RemoveMedicalHistoryRecordCommandHandler(IPatientRepository _patientRepository, IUnitOfWork _unitOfWork) : IRequestHandler<RemoveMedicalHistoryRecordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RemoveMedicalHistoryRecordCommand request, CancellationToken cancellationToken)
    {
        var patientId = PatientId.Create(request.PatientId);
        if (patientId.IsFailure) return Result.Failure<string>(patientId.Error!);

        var patient = await _patientRepository.GetPatientDetails(patientId.Value!);
        if (patient is null) return Result.Failure<string>(PatientErrors.NotFound);

        var medicalHistoryIdResult = MedicalHistoryId.Create(request.MedicalHistoryRecordId);
        if(medicalHistoryIdResult.IsFailure) return Result.Failure<string>(medicalHistoryIdResult.Error!);

         var result = patient.RemoveMedicalHistoryRecord(medicalHistoryIdResult.Value!);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}

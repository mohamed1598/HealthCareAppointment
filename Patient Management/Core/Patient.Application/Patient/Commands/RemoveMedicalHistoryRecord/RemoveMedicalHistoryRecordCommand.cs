using MediatR;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Commands.RemoveMedicalHistoryRecord;

public record RemoveMedicalHistoryRecordCommand(Guid PatientId, Guid MedicalHistoryRecordId):IRequest<Result<string>>;
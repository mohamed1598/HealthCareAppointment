using MediatR;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Application.Patient.Commands.AddMedicalHistoryRecord;

public record AddMedicalHistoryRecordCommand(Guid PatientId, string Diagnosis, string Treatment, DateTime? Date):IRequest<Result<string>>;
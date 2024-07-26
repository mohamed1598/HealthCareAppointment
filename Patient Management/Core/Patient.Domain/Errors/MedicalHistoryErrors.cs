using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.Errors;

public class MedicalHistoryErrors
{
    public static readonly Error NotFound = new Error(
            "MedicalHistory.NotFound",
            "Medical History does not exist.");
}

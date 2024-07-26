using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.Errors;

public static class PatientErrors
{
    public static readonly Error NotFound = new Error(
            "Patient.NotFound",
            "Patient does not exist.");
}

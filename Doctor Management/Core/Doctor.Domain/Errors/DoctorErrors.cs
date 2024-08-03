using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Errors;

public static class DoctorErrors
{
    public static readonly Error NotFound = new Error(
            "Doctor.NotFound",
            "Doctor does not exist.");
}

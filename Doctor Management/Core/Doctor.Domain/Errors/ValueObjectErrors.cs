using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Errors;

public static class ValueObjectErrors
{
    public static class UserId
    {
        public static readonly Error Empty = new Error(
        "UserId.Empty",
        "UserId is empty.");

        public static readonly Error NotValid = new Error(
            "UserId.NotValid",
            "UserId is not valid.");

    }
}

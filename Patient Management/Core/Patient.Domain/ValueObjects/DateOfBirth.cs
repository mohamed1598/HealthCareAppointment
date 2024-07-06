using Patient.Domain.Errors;
using Shared.Primitives;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.ValueObjects
{
    public record DateOfBirth : ValueObject
    {
        public const int MaxAge = 120;

        public DateOfBirth(DateTime value) => Value = value;
        public DateTime Value { get; }

        public static Result<DateOfBirth> Create(DateTime date)
        {
            if (date.AddYears(MaxAge) > DateTime.Today)
                return Result.Failure<DateOfBirth>(ValueObjectErrors.DateOfBirth.Invalid);

            return new DateOfBirth(date);
        }
    }
}

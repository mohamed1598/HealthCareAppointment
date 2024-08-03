using Doctor.Application.Doctor.Commands.RegisterDoctor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doctor.Application.Doctor.Commands.UpdateDoctor;

public partial class UpdateDoctorCommandValidator : AbstractValidator<UpdateDoctorCommand>
{

    public UpdateDoctorCommandValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(d => d.IsAvailable)
            .NotNull()
            .WithMessage("Availability must be specified.");

        RuleFor(d => d.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(ValidPhoneNumberRegex())
            .WithMessage("Phone number must start with 011, 010, 012, or 015 followed by 8 digits.");

        RuleFor(d => d.Address)
            .NotEmpty()
            .WithMessage("Address is required.");

        RuleFor(d => d.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email is required.");

        RuleFor(d => d.Specialization)
            .NotEmpty()
            .WithMessage("Specialization is required.");
    }

    [GeneratedRegex(@"^(011|010|012|015)[0-9]{8}$", RegexOptions.Compiled)]
    private static partial Regex ValidPhoneNumberRegex();
}

using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.Errors;

public static class ValueObjectErrors
{
    public static class PatientId
    {
        public static readonly Error NotValid = new Error(
            "MemberId.NotValid",
            "Member Id is not valid.");
    }
    public static class MedicalHistoryId
    {
        public static readonly Error NotValid = new Error(
            "MedicalHistoryId.NotValid",
            "Medical History Id is not valid.");
    }
    public static class Name
    {
        public static readonly Error Empty = new Error(
        "Name.Empty",
        "Name is empty.");

        public static readonly Error TooLong = new Error(
            "Name.TooLong",
            "Name is too long.");

    }
    public static class DateOfBirth
    {
        public static readonly Error Invalid = new Error(
            "DateOfBirth.Invalid",
            "Date Of Birth is invalid.");

    }

    public static class PhoneNumber
    {
        public static readonly Error Empty = new Error(
            "PhoneNumber.Empty",
            "Phone number is required.");

        public static readonly Error InvalidLength = new Error(
            "PhoneNumber.InvalidLength",
            "Phone number must be exactly 11 characters long.");

        public static readonly Error InvalidFormat = new Error(
            "PhoneNumber.Empty",
            "Phone number format is invalid.");

    }

    public static class Email
    {
        public static readonly Error Empty = new Error(
            "Email.Empty",
            "Email is required.");

        public static readonly Error InvalidFormat = new Error(
            "Email.Empty",
            "Email format is invalid.");

    }

    public static class Address
    {
        public static readonly Error Empty = new Error(
        "Address.Empty",
        "Address is empty.");

        public static readonly Error TooLong = new Error(
            "Address.TooLong",
            "Address is too long.");

    }

    public static class Diagnosis
    {
        public static readonly Error Empty = new Error(
        "Diagnosis.Empty",
        "Diagnosis is empty.");

        public static readonly Error TooLong = new Error(
            "Diagnosis.TooLong",
            "Diagnosis is too long.");

    }

    public static class Treatment
    {
        public static readonly Error Empty = new Error(
        "Treatment.Empty",
        "Treatment is empty.");

        public static readonly Error TooLong = new Error(
            "Treatment.TooLong",
            "Treatment is too long.");

    }

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

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Patient.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient.Domain.ValueObjects;
using System.Reflection.Emit;

namespace Patient.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Entities.Patient>
{
    public void Configure(EntityTypeBuilder<Entities.Patient> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                patientId => patientId.Value,
                value => new PatientId(value)
            );

        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value
            );

        builder.Property(p => p.DateOfBirth)
            .HasConversion(
                dateOfBirth => dateOfBirth.Value,
                value => DateOfBirth.Create(value).Value
            );

        builder.Property(p => p.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value)
            );

        builder.OwnsOne(p => p.ContactDetails, contactDetails =>
        {
            contactDetails.Property(cd => cd.PhoneNumber)
                .HasConversion(
                    phoneNumber => phoneNumber.Value,
                    value => PhoneNumber.Create(value).Value
                )
                .HasColumnName("PhoneNumber");

            contactDetails.Property(cd => cd.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value
                )
                .HasColumnName("Email");

            contactDetails.Property(cd => cd.Address)
                .HasConversion(
                    address => address.Value,
                    value => Address.Create(value).Value
                )
                .HasColumnName("Address");
        });

        builder
            .HasMany(p => p.MedicalHistories)
            .WithOne()
            .HasForeignKey("PatientId");

        builder
            .Metadata
            .FindNavigation(nameof(Domain.Entities.Patient.MedicalHistories))?
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
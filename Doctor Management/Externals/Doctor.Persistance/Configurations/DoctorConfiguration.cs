using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities = Doctor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Doctor.Domain.ValueObjects;

namespace Doctor.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Entities.Doctor>
{
    public void Configure(EntityTypeBuilder<Entities.Doctor> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(
                doctorId => doctorId.Value,
                value => new DoctorId(value)
            );

        builder.Property(p => p.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value)
            );

        builder.Property(p => p.UserId)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value)
            );

        builder.Property(p => p.Specialization)
            .HasConversion(
                specialization => specialization.Value,
                value => Specialization.Create(value)
            );

        builder.OwnsOne(p => p.ContactDetails, contactDetails =>
        {
            contactDetails.Property(cd => cd.PhoneNumber)
                .HasConversion(
                    phoneNumber => phoneNumber.Value,
                    value => PhoneNumber.Create(value)
                )
                .HasColumnName("PhoneNumber");

            contactDetails.Property(cd => cd.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value)
                )
                .HasColumnName("Email");

            contactDetails.Property(cd => cd.Address)
                .HasConversion(
                    address => address.Value,
                    value => Address.Create(value)
                )
                .HasColumnName("Address");
        });
    }
}
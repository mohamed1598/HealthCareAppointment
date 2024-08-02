using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Patient.Domain.Entities;
using Patient.Domain.ValueObjects;

namespace Patient.Persistence.Configurations;

public class MedicalHistoryConfiguration : IEntityTypeConfiguration<MedicalHistory>
{
    public void Configure(EntityTypeBuilder<MedicalHistory> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                medicalHistoryId => medicalHistoryId.Value,
                value => new MedicalHistoryId(value)
            );

        builder.Property(m => m.Diagnosis)
            .HasConversion(
                firstName => firstName.Value,
                value => Diagnosis.Create(value).Value!
            );

        builder.Property(m => m.Treatment)
            .HasConversion(
                lastName => lastName.Value,
                value => Treatment.Create(value).Value!
            );
    }
}

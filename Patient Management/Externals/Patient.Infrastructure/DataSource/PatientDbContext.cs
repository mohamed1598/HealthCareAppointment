using Microsoft.EntityFrameworkCore;
using Entities = Patient.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Infrastructure.DataSource
{
    public class PatientDbContext(DbContextOptions<PatientDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.Patient> Patients { get; set; }
        public DbSet<Entities.MedicalHistory> MedicalHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PatientDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

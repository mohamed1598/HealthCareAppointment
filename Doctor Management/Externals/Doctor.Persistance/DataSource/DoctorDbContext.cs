using Doctor.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Entities = Doctor.Domain.Entities;

namespace Doctor.Persistence.DataSource
{
    public class DoctorDbContext(DbContextOptions<DoctorDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.Doctor> Doctors { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DoctorDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}

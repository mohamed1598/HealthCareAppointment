using Doctor.Domain.Abstractions;

namespace Doctor.Persistence.DataSource;

public class UnitOfWork(DoctorDbContext _context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

using Doctor.Domain.Abstractions;
using Doctor.Domain.ValueObjects;
using Doctor.Persistence.DataSource;
using Microsoft.EntityFrameworkCore;

namespace Doctor.Persistence.Repositories;

public class DoctorRepository(DoctorDbContext context) : IDoctorRepository
{
    private readonly DoctorDbContext _context = context;
    public void Add(Doctor.Domain.Entities.Doctor patient)
    {
        _context.Add(patient);
    }

    public async Task<Doctor.Domain.Entities.Doctor?> GetDoctorDetails(DoctorId id)
    {
        return await _context.Doctors.Where(p => p.Id == id && !p.IsDeleted).FirstOrDefaultAsync();
    }
}

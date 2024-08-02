using Microsoft.EntityFrameworkCore;
using Patient.Domain.Abstractions;
using Patient.Domain.ValueObjects;
using Patient.Persistence.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Persistence.Repositories;

public class PatientRepository(PatientDbContext context) : IPatientRepository
{
    private readonly PatientDbContext _context = context;
    public void Add(Domain.Entities.Patient patient)
    {
        _context.Add(patient);
    }

    public Domain.Entities.Patient? GetPatientById(PatientId id)
    {
        return _context.Patients.FirstOrDefault(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<Domain.Entities.Patient?> GetPatientDetails(PatientId id)
    {
        return await _context.Patients.Where(p => p.Id == id && !p.IsDeleted).Include(p => p.MedicalHistories).FirstOrDefaultAsync();
    }
}

using Patient.Domain.Abstractions;
using Patient.Domain.ValueObjects;
using Patient.Infrastructure.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Infrastructure.Repositories;

public class PatientRepository(PatientDbContext context) : IPatientRepository
{
    private readonly PatientDbContext _context = context;
    public void Add(Domain.Entities.Patient patient)
    {
        _context.Add(patient);
    }

    public Domain.Entities.Patient? GetPatientById(PatientId id)
    {
        return _context.Patients.Find(id);
    }
}

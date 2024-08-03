using Doctor.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor.Domain.Abstractions;

public interface IDoctorRepository
{
    public void Add(Entities.Doctor doctor);
    public Task<Entities.Doctor?> GetDoctorDetails(DoctorId id);
}
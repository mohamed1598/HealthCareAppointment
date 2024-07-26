
using Patient.Domain.Entities;
using Patient.Domain.ValueObjects;

namespace Patient.Domain.Abstractions;

public interface IPatientRepository 
{ 
    public void Add(Entities.Patient patient);
    public Entities.Patient? GetPatientById(PatientId id);
}

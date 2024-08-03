using Patient.Domain.ValueObjects;
using Shared.Primitives;
using Shared.Result;

namespace Patient.Domain.Entities
{
    public class MedicalHistory : Entity<MedicalHistoryId>
    {
        protected MedicalHistory() : base(MedicalHistoryId.Create(Guid.NewGuid()).Value!)
        {
        }
        public Diagnosis Diagnosis { get; set; }
        public Treatment Treatment { get; set; }
        public DateTime Date { get; set; }
        public PatientId PatientId { get; set; }

        public MedicalHistory(MedicalHistoryId id,Diagnosis diagnosis, Treatment treatment, DateTime date,PatientId patientId) : base(id) 
        {
            Diagnosis = diagnosis;
            Treatment = treatment;
            Date = date;
            PatientId = patientId;
        }

        public static Result<MedicalHistory> Create(Diagnosis diagnosis, Treatment treatment,DateTime date,PatientId patientId)
        {
            var MedicalHistoryIdResult = MedicalHistoryId.Create(Guid.NewGuid());
            if (MedicalHistoryIdResult.IsFailure)
                return Result.Failure<MedicalHistory>(MedicalHistoryIdResult.Error!);
            
            return new MedicalHistory(MedicalHistoryIdResult.Value!, diagnosis, treatment, date,patientId);

        }
    }
}
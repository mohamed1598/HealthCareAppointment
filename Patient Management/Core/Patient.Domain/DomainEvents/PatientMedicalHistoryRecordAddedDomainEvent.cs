using Patient.Domain.Entities;
using Patient.Domain.ValueObjects;
using Shared.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient.Domain.DomainEvents;

public record PatientMedicalHistoryRecordAddedDomainEvent(PatientId PatientId, MedicalHistory History) : IDomainEvent;

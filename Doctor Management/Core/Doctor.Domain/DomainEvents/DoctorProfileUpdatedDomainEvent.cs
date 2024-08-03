using Doctor.Domain.Entities;
using Doctor.Domain.ValueObjects;
using Shared.Primitives;

namespace Doctor.Domain.DomainEvents;

public record DoctorProfileUpdatedDomainEvent(DoctorId DoctorId, Name Name, Availability Availability, ContactDetails ContactDetails, Specialization Specialization) : IDomainEvent;

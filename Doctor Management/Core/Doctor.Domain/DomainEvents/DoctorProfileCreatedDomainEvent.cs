using Doctor.Domain.Entities;
using Doctor.Domain.ValueObjects;
using Shared.Primitives;

namespace Doctor.Domain.DomainEvents;

public record DoctorProfileCreatedDomainEvent(DoctorId DoctorId, Name Name, Availability Availability, ContactDetails ContactDetails, Specialization Specialization, UserId UserId) : IDomainEvent;


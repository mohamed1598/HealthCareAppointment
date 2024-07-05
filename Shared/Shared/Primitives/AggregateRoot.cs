namespace Shared.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected AggregateRoot(TId id) : base(id)
    {
    }
    public ICollection<IDomainEvent> DomainEvents => _domainEvents;

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

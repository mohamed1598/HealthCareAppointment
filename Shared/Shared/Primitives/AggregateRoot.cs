namespace Shared.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId> where TId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = [];
    protected AggregateRoot(TId id) : base(id)
    {
    }
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>   _domainEvents.Add(domainEvent);
}

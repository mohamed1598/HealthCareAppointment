using Doctor.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Shared.Primitives;

namespace Doctor.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptors : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if(dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = dbContext
            .ChangeTracker
            .Entries()
            .Where(e => e.Entity.GetType().BaseType?.IsGenericType == true &&
                e.Entity.GetType().BaseType?.GetGenericTypeDefinition() == typeof(AggregateRoot<>))
            .Select(x => x.Entity).
            SelectMany(aggregateRoot =>
            {
                var methodName = nameof(AggregateRoot<ValueObject>.GetDomainEvents);
                var methodInfo = aggregateRoot.GetType().GetMethod(methodName);
                return methodInfo?.Invoke(aggregateRoot, null) as IEnumerable<IDomainEvent> ?? Enumerable.Empty<IDomainEvent>();
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All})
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}

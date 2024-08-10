using EventStore.Client;
using Newtonsoft.Json;
using Shared.Primitives;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using ReadState = EventStore.Client.ReadState;

namespace Appointments.API.EventStore;

public partial class EsAggregateStore : IAggregateStore
{
    private readonly EventStoreClient _connection;

    public EsAggregateStore(EventStoreClient connection) => _connection = connection;

    public async Task Save<T, TId>(T aggregate)
        where T : AggregateRoot<TId>  
        where TId : ValueObject
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        var changes = aggregate.GetDomainEvents().ToArray();

        if (!changes.Any()) return;

        var streamName = GetStreamName<T,TId>(aggregate);
        await _connection.AppendEvents(
            streamName,
            1,
            changes);

        aggregate.ClearDomainEvents();
    }

    public async Task<T> Load<T,TId>(TId aggregateId)
        where T : AggregateRoot<TId>
        where TId : ValueObject
    {
        if (aggregateId == null)
            throw new ArgumentNullException(nameof(aggregateId));

        var stream = GetStreamName<T,TId>(aggregateId);
        var aggregate = (T)Activator.CreateInstance(typeof(T), true)!;

        var page = await _connection.ReadStreamAsync(Direction.Forwards,
        stream,
        StreamPosition.Start).ToListAsync();

        aggregate!.RebuildFromEvents(page.Select(resolvedEvent => resolvedEvent.Deserialze() as IDomainEvent).ToArray()!);

        return aggregate;
    }

    public async Task<bool> Exists<T,TId>(TId aggregateId)
        where T : AggregateRoot<TId>
        where TId : ValueObject
    {
        var stream = GetStreamName<T,TId>(aggregateId);
        var result = _connection.ReadStreamAsync(
            Direction.Forwards,
            stream,
            10,
            20
        );

        return await result.ReadState != ReadState.StreamNotFound;
    }

    private static byte[] Serialize(object data)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

    private static string GetStreamName<T,TId>(ValueObject aggregateId)
        where T : AggregateRoot<TId>
        where TId : ValueObject
        => $"{typeof(T).Name}-{aggregateId}";

    private static string GetStreamName<T,TId>(T aggregate)
        where T : AggregateRoot<TId>
        where TId : ValueObject
        => $"{typeof(T).Name}-{aggregate.Id}";
}
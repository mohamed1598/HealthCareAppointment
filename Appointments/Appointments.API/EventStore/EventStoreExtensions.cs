using EventStore.Client;
using Newtonsoft.Json;
using System.Text;

namespace Appointments.API.EventStore;

public static class EventStoreExtensions
{
    public static Task AppendEvents(this EventStoreClient connection,
                string streamName, long version,
                object[] events)
    {
        if (events == null || !events.Any()) return Task.CompletedTask;

        var preparedEvents = events
            .Select(@event =>
                new EventData(
                    eventId: Uuid.NewUuid(),
                    type: @event.GetType().Name,
                    data: Serialize(@event),
                    metadata: Serialize(new EventMetadata
                    { ClrType = @event.GetType().AssemblyQualifiedName })
                ))
            .ToArray();
        return connection.AppendToStreamAsync(
            streamName,
            StreamState.Any,
            preparedEvents);
    }

    private static byte[] Serialize(object data)
        => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
}
﻿using EventStore.Client;
using Newtonsoft.Json;
using System.Text;

namespace Appointments.API.EventStore;

public static class EventDeserializer
{
    public static object Deserialze(this ResolvedEvent resolvedEvent)
    {
        var meta = JsonConvert.DeserializeObject<EventMetadata>(
                Encoding.UTF8.GetString(resolvedEvent.Event.Metadata.ToArray()));
        var dataType = Type.GetType(meta.ClrType);
        var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
        var data = JsonConvert.DeserializeObject(jsonData, dataType);
        return data;
    }

    public static T Deserialze<T>(this ResolvedEvent resolvedEvent)
    {
        var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
        return JsonConvert.DeserializeObject<T>(jsonData);
    }
}
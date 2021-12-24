﻿using Domain.Aggregates;
using ECommerce.JsonConverters;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Infrastructure.EventSourcing.EventStore.Contexts.Converters;

public class InventoryItemConverter : ValueConverter<InventoryItem, string> 
{
    public InventoryItemConverter()
        : base(
            @event => JsonConvert.SerializeObject(@event, typeof(InventoryItem), SerializerSettings()),
            jsonString => JsonConvert.DeserializeObject<InventoryItem>(jsonString, DeserializerSettings())) { }

    private static JsonSerializerSettings SerializerSettings()
    {
        var jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        jsonSerializerSettings.Converters.Add(new DateOnlyJsonConverter());
        jsonSerializerSettings.Converters.Add(new ExpirationDateOnlyJsonConverter());

        return jsonSerializerSettings;
    }

    private static JsonSerializerSettings DeserializerSettings()
    {
        var jsonDeserializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new PrivateSetterContractResolver()
        };

        jsonDeserializerSettings.Converters.Add(new DateOnlyJsonConverter());
        jsonDeserializerSettings.Converters.Add(new ExpirationDateOnlyJsonConverter());

        return jsonDeserializerSettings;
    }
}
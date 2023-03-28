using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Json;

public class LookupJsonConverter : JsonConverter {
    private readonly Lazy<ILookups> _lookups;

    public LookupJsonConverter(Lazy<ILookups> lookups) {
        _lookups = lookups;
    }

    public override bool CanConvert(Type objectType) {
        return objectType.ImplementsInterface<ILookup>();
    }

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            return null;
        }

        if (reader.TokenType != JsonToken.String) {
            throw new JsonSerializationException();
        }

        var lookupId = (string) reader.Value;

        var lookup = _lookups.Value.FindById(objectType, lookupId) ?? _lookups.Value.FindByName(objectType, lookupId);

        return lookup;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var lookup = value as ILookup;

        if (lookup == null) {
            return;
        }

        var jValue = new JValue(lookup.Id);

        jValue.WriteTo(writer);
    }
}

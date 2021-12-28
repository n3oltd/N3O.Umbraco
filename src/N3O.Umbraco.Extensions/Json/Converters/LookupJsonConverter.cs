using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Json;

public class LookupJsonConverter : JsonConverter {
    private readonly ILookups _lookups;
    
    public LookupJsonConverter(ILookups lookups) {
        _lookups = lookups;
    }

    public override bool CanConvert(Type objectType) {
        var canConvert = objectType.ImplementsInterface<ILookup>();

        return canConvert;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null || reader.Value == null) {
            return null;
        }

        if (reader.TokenType != JsonToken.Integer) {
            throw new JsonSerializationException();
        }

        var lookupId = (string) reader.Value;

        return _lookups.FindById(objectType, lookupId);
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

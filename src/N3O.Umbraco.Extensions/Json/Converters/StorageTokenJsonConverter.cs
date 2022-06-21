using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Json;

public class StorageTokenJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        var canConvert = objectType == typeof(StorageToken);

        return canConvert;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType != JsonToken.String || reader.Value == null) {
            return null;
        }

        var base64EncodedData = reader.Value.ToString();

        try {
            var storageToken = StorageToken.FromBase64String(base64EncodedData);

            return storageToken;
        } catch {
            return null;
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            return;
        }

        var storageToken = (StorageToken)value;
        var base64EncodedData = storageToken.ToBase64String();

        var jValue = new JValue(base64EncodedData);

        jValue.WriteTo(writer);
    }
}

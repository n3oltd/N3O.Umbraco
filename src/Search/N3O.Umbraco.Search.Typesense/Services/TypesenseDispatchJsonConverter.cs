using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseDispatchJsonConverter : JsonConverter {
    public static readonly TypesenseDispatchJsonConverter Instance = new();

    private TypesenseDispatchJsonConverter() { }

    public override bool CanConvert(Type objectType) {
        return TypesenseConverterRegistry.HasConverterFor(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();

            return;
        }

        var converter = TypesenseConverterRegistry.GetConverter(value.GetType());
        var typesenseValue = converter.ToTypesenseValue(value);

        if (typesenseValue == null) {
            writer.WriteNull();
        } else {
            serializer.Serialize(writer, typesenseValue);
        }
    }

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            return null;
        }

        var converter = TypesenseConverterRegistry.GetConverter(objectType);
        var token = JToken.Load(reader);

        var typesenseValue = token.Type == JTokenType.Null
                                 ? null
                                 : token.ToObject(converter.UnderlyingTypesenseType, serializer);

        return converter.FromTypesenseValue(typesenseValue);
    }
}

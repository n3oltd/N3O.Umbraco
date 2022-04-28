using Humanizer.Bytes;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Json {
    public class ByteSizeJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            var canConvert = objectType.IsOfTypeOrNullableType<ByteSize>();

            return canConvert;
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null || reader.Value == null) {
                return null;
            }

            if (reader.TokenType != JsonToken.Integer) {
                throw new JsonSerializationException();
            }

            var value = Double.Parse(reader.Value.ToString());

            return ByteSize.FromBytes(value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value == null) {
                return;
            }

            var byteSize = (ByteSize) value;

            var jValue = new JValue((long) byteSize.Bytes);

            jValue.WriteTo(writer);
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Net;

namespace N3O.Umbraco.Json {
    public class IPAddressJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(IPAddress);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            if (reader.Value == null) {
                return null;
            } else {
                return IPAddress.Parse((string) reader.Value);
            }
        }
    }
}
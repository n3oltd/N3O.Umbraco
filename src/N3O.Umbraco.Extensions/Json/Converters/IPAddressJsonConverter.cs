using Newtonsoft.Json;
using System;
using System.Net;
using Umbraco.Extensions;

namespace N3O.Umbraco.Json;

public class IPAddressJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType.IsAssignableTo(typeof(IPAddress));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        var str = (string) reader.Value;

        return str.IfNotNull(IPAddress.Parse);
    }
}

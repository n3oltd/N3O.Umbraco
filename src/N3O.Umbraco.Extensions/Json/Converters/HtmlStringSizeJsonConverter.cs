using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Json;

public class HtmlStringJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return typeof(HtmlEncodedString).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        var source = value as HtmlEncodedString;

        if (source == null) {
            return;
        }

        writer.WriteValue(source.ToString());
    }

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        var str = (string) reader.Value;

        return str.HasValue() ? null : new HtmlEncodedString(str);
    }
}
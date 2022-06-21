using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Json;

public class SerializeToUrlJsonConverter : JsonConverter {
    private readonly Lazy<IUrlBuilder> _urlBuilder;

    public SerializeToUrlJsonConverter(Lazy<IUrlBuilder> urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public override bool CanRead => false;

    public override bool CanConvert(Type objectType) {
        return objectType.HasCustomAttribute<SerializeToUrlAttribute>(true);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value == null) {
            return;
        }

        var jObject = GetJObject(serializer, value);
        
        foreach (var attribute in value.GetType().GetCustomAttributes<SerializeToUrlAttribute>(true)) {
            var jProperty = jObject.Properties().SingleOrDefault(x => x.Name.EqualsInvariant(attribute.Property));
            var jToken = jProperty?.Value;

            if (jToken?.Type != JTokenType.String) {
                throw new Exception($"Could not find a string property with name {attribute.Property.Quote()} on type {value.GetType().GetFriendlyName()}");
            }

            jObject[jProperty.Name] = GetUrl((string) jToken);
        }

        jObject.WriteTo(writer);
    }

    private JObject GetJObject(JsonSerializer serializer, object value) {
        using (var stringWriter = new StringWriter()) {
            using (var jsonWriter = new JsonTextWriter(stringWriter)) {
                serializer.Converters.Remove(this);
                serializer.Serialize(jsonWriter, value);
                serializer.Converters.Add(this);

                return JObject.Parse(stringWriter.ToString());
            }
        }
    }
    
    private JValue GetUrl(string path) {
        return new JValue(_urlBuilder.Value.Root().AppendPathSegment(path));
    }
}

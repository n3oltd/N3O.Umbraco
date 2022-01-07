using Humanizer;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Json {
    public class PublishedElementJsonConverter : JsonConverter {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) {
            return objectType.ImplementsInterface<IPublishedElement>();
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value != null) {
                var publishedElement = (IPublishedElement) value;

                writer.WriteStartObject();

                writer.WritePropertyName(nameof(IPublishedElement.Key).Camelize());
                writer.WriteValue(publishedElement.Key);

                writer.WritePropertyName(nameof(IPublishedElement.ContentType).Camelize());
                writer.WriteValue(publishedElement.ContentType.Alias);

                foreach (var property in publishedElement.Properties) {
                    writer.WritePropertyName(property.Alias);
                    serializer.Serialize(writer, property.GetValue());
                }

                writer.WriteEndObject();
            }
        }
    }
}
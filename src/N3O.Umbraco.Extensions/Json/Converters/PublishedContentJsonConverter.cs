using Humanizer;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Json {
    public class PublishedContentJsonConverter : JsonConverter {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) {
            return objectType.ImplementsInterface<IPublishedContent>();
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                        object existingValue,
                                        JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value != null) {
                var publishedContent = (IPublishedContent) value;

                writer.WriteStartObject();

                writer.WritePropertyName(nameof(IPublishedContent.Id).Camelize());
                writer.WriteValue(publishedContent.Id);

                writer.WritePropertyName(nameof(IPublishedContent.Key).Camelize());
                writer.WriteValue(publishedContent.Key);

                writer.WritePropertyName(nameof(IPublishedContent.ContentType).Camelize());
                writer.WriteValue(publishedContent.ContentType.Alias);
                
                writer.WritePropertyName(nameof(IPublishedContent.Name).Camelize());
                writer.WriteValue(publishedContent.Name);

                writer.WritePropertyName(nameof(IPublishedContent.CreateDate).Camelize());
                writer.WriteValue(publishedContent.CreateDate);

                writer.WritePropertyName(nameof(IPublishedContent. UpdateDate).Camelize());
                writer.WriteValue(publishedContent.UpdateDate);

                foreach (var property in publishedContent.Properties) {
                    writer.WritePropertyName(property.Alias);
                    serializer.Serialize(writer, property.GetValue());
                }

                writer.WriteEndObject();
            }
        }
    }
}
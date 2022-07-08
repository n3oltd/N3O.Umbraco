using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Json;

public class PublishedContentJsonConverter : JsonConverter {
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IUrlBuilder> _urlBuilder;

    public PublishedContentJsonConverter(Lazy<IUserService> userService, Lazy<IUrlBuilder> urlBuilder) {
        _userService = userService;
        _urlBuilder = urlBuilder;
    }
    
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

            writer.WritePropertyName(nameof(ContentRes.Id).Camelize());
            writer.WriteValue(publishedContent.Id);

            writer.WritePropertyName(nameof(ContentRes.Key).Camelize());
            writer.WriteValue(publishedContent.Key);
            
            writer.WritePropertyName(nameof(ContentRes.Url).Camelize());
            writer.WriteValue(_urlBuilder.Value.Root().AppendPathSegment(publishedContent.Url()).ToString());
            
            writer.WritePropertyName(nameof(ContentRes.Level).Camelize());
            writer.WriteValue(publishedContent.Level);
            
            writer.WritePropertyName(nameof(ContentRes.CreateDate).Camelize());
            writer.WriteValue(publishedContent.CreateDate);

            writer.WritePropertyName(nameof(ContentRes.UpdateDate).Camelize());
            writer.WriteValue(publishedContent.UpdateDate);
            
            writer.WritePropertyName(nameof(ContentRes.CreatorName).Camelize());
            writer.WriteValue(publishedContent.GetCreatorName(_userService.Value));

            writer.WritePropertyName(nameof(ContentRes.WriterName).Camelize());
            writer.WriteValue(publishedContent.GetWriterName(_userService.Value));

            writer.WritePropertyName(nameof(ContentRes.Name).Camelize());
            writer.WriteValue(publishedContent.Name);
            
            writer.WritePropertyName(nameof(ContentRes.ParentId).Camelize());
            writer.WriteValue(publishedContent.Parent?.Id);

            writer.WritePropertyName(nameof(ContentRes.SortOrder).Camelize());
            writer.WriteValue(publishedContent.SortOrder);
            
            writer.WritePropertyName(nameof(ContentRes.ContentTypeAlias).Camelize());
            writer.WriteValue(publishedContent.ContentType.Alias);

            foreach (var property in publishedContent.Properties) {
                writer.WritePropertyName(property.Alias);
                serializer.Serialize(writer, property.GetValue());
            }

            writer.WriteEndObject();
        }
    }
}

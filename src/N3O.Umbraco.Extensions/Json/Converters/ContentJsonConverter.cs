using Humanizer;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Json;

public class ContentJsonConverter : JsonConverter {
    private readonly Lazy<PropertyValueConverterCollection> _propertyValueConverters;
    private readonly Lazy<IUmbracoContextFactory> _umbracoContextFactory;
    private readonly Lazy<IPublishedModelFactory> _publishedModelFactory;
    private readonly Lazy<IPublishedContentTypeFactory> _contentTypeFactory;
    private readonly Lazy<IUserService> _userService;

    public ContentJsonConverter(Lazy<PropertyValueConverterCollection> propertyValueConverters,
                                Lazy<IUmbracoContextFactory> umbracoContextFactory,
                                Lazy<IPublishedModelFactory> publishedModelFactory,
                                Lazy<IPublishedContentTypeFactory> contentTypeFactory,
                                Lazy<IUserService> userService) {
        _propertyValueConverters = propertyValueConverters;
        _umbracoContextFactory = umbracoContextFactory;
        _publishedModelFactory = publishedModelFactory;
        _contentTypeFactory = contentTypeFactory;
        _userService = userService;
    }

    public override bool CanRead => false;
    
    public override bool CanConvert(Type objectType) {
        return objectType.ImplementsInterface<IContent>();
    }

    public override object ReadJson(JsonReader reader,
                                    Type objectType,
                                    object existingValue,
                                    JsonSerializer serializer) {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value != null) {
            var content = (IContent) value;

            writer.WriteStartObject();

            writer.WritePropertyName(nameof(ContentRes.Id).Camelize());
            writer.WriteValue(content.Id);

            writer.WritePropertyName(nameof(ContentRes.Key).Camelize());
            writer.WriteValue(content.Key);
            
            writer.WritePropertyName(nameof(ContentRes.Url).Camelize());
            writer.WriteNull();
            
            writer.WritePropertyName(nameof(ContentRes.Level).Camelize());
            writer.WriteValue(content.Level);
            
            writer.WritePropertyName(nameof(ContentRes.CreateDate).Camelize());
            writer.WriteValue(content.CreateDate);

            writer.WritePropertyName(nameof(ContentRes.UpdateDate).Camelize());
            writer.WriteValue(content.UpdateDate);
            
            writer.WritePropertyName(nameof(ContentRes.CreatorName).Camelize());
            writer.WriteValue(_userService.Value.GetUserById(content.CreatorId)?.Name);

            writer.WritePropertyName(nameof(ContentRes.WriterName).Camelize());
            writer.WriteValue(_userService.Value.GetUserById(content.WriterId)?.Name);

            writer.WritePropertyName(nameof(ContentRes.Name).Camelize());
            writer.WriteValue(content.Name);
            
            writer.WritePropertyName(nameof(ContentRes.ParentId).Camelize());
            writer.WriteValue(content.ParentId);

            writer.WritePropertyName(nameof(ContentRes.SortOrder).Camelize());
            writer.WriteValue(content.SortOrder);
            
            writer.WritePropertyName(nameof(ContentRes.ContentTypeAlias).Camelize());
            writer.WriteValue(content.ContentType.Alias);

            foreach (var property in content.Properties) {
                writer.WritePropertyName(property.Alias);
                serializer.Serialize(writer, GetPublishedValue(content.ContentType.Alias, property));
            }

            writer.WriteEndObject();
        }
    }

    private object GetPublishedValue(string contentTypeAlias, IProperty property) {
        var propertyValue = property.GetValue();

        if (propertyValue == null) {
            return null;
        }

        var umbracoContext = _umbracoContextFactory.Value.EnsureUmbracoContext().UmbracoContext;
    
        var contentType =  umbracoContext.PublishedSnapshot.Content.GetContentType(contentTypeAlias);
        var publishedPropertyType = new PublishedPropertyType(contentType,
                                                              property.PropertyType,
                                                              _propertyValueConverters.Value,
                                                              _publishedModelFactory.Value,
                                                              _contentTypeFactory.Value);

        var converter = _propertyValueConverters.Value
                                                .Where(x => x is not MustBeStringValueConverter &&
                                                            x.IsConverter(publishedPropertyType))
                                                .MinBy(x => x is JsonValueConverter ? 1 : 0);

        var intermediate = converter.ConvertSourceToIntermediate(null,
                                                                 publishedPropertyType,
                                                                 propertyValue,
                                                                 false);

        return converter.ConvertIntermediateToObject(null,
                                                     publishedPropertyType,
                                                     PropertyCacheLevel.None,
                                                     intermediate,
                                                     false);
    }
}

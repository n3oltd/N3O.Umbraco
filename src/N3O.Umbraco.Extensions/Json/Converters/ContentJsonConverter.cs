using Humanizer;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Json {
    public class ContentJsonConverter : JsonConverter {
        private readonly Lazy<PropertyValueConverterCollection> _propertyValueConverters;
        private readonly Lazy<IUmbracoContextAccessor> _umbracoContextAccessor;
        private readonly Lazy<IPublishedModelFactory> _publishedModelFactory;
        private readonly Lazy<IPublishedContentTypeFactory> _publishedContentTypeFactory;

        public ContentJsonConverter(Lazy<PropertyValueConverterCollection> propertyValueConverters,
                                    Lazy<IUmbracoContextAccessor> umbracoContextAccessor,
                                    Lazy<IPublishedModelFactory> publishedModelFactory,
                                    Lazy<IPublishedContentTypeFactory> publishedContentTypeFactory) {
            _propertyValueConverters = propertyValueConverters;
            _umbracoContextAccessor = umbracoContextAccessor;
            _publishedModelFactory = publishedModelFactory;
            _publishedContentTypeFactory = publishedContentTypeFactory;
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

                writer.WritePropertyName(nameof(IContent.Id).Camelize());
                writer.WriteValue(content.Id);

                writer.WritePropertyName(nameof(IContent.Key).Camelize());
                writer.WriteValue(content.Key);

                writer.WritePropertyName(nameof(IContent.ContentType).Camelize());
                writer.WriteValue(content.ContentType.Alias);
                
                writer.WritePropertyName(nameof(IContent.Name).Camelize());
                writer.WriteValue(content.Name);

                writer.WritePropertyName(nameof(IContent.CreateDate).Camelize());
                writer.WriteValue(content.CreateDate);

                writer.WritePropertyName(nameof(IContent. UpdateDate).Camelize());
                writer.WriteValue(content.UpdateDate);

                foreach (var property in content.Properties) {
                    writer.WritePropertyName(property.Alias);
                    serializer.Serialize(writer, GetPublishedValue(content.ContentType.Alias, property));
                }

                writer.WriteEndObject();
            }
        }

        private object GetPublishedValue(string contentTypeAlias, IProperty property) {
            _umbracoContextAccessor.Value.TryGetUmbracoContext(out var umbracoContext);
        
            var publishedContentType =  umbracoContext.PublishedSnapshot.Content.GetContentType(contentTypeAlias);
            var publishedPropertyType = new PublishedPropertyType(publishedContentType,
                                                                  property.PropertyType,
                                                                  _propertyValueConverters.Value,
                                                                  _publishedModelFactory.Value,
                                                                  _publishedContentTypeFactory.Value);

            var converter = _propertyValueConverters.Value
                                                    .FirstOrDefault(x => x is not MustBeStringValueConverter &&
                                                                         x.IsConverter(publishedPropertyType));

            var intermediate = converter.ConvertSourceToIntermediate(null,
                                                                     publishedPropertyType,
                                                                     property.GetValue(),
                                                                     false);

            return converter.ConvertIntermediateToObject(null,
                                                         publishedPropertyType,
                                                         PropertyCacheLevel.None,
                                                         intermediate,
                                                         false);
        }
    }
}
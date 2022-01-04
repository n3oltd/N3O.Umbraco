using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.SerpEditor.Models;
using N3O.Umbraco.ValueConverters;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.SerpEditor.DataTypes {
    public class SerpEditorValueConverter : PropertyValueConverter {
        private readonly IJsonProvider _jsonProvider;

        public SerpEditorValueConverter(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.EqualsInvariant(SerpEditorConstants.PropertyEditorAlias);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                           IPublishedPropertyType propertyType,
                                                           object source,
                                                           bool preview) {
            SerpEntry entry;

            if (source is string json && json.HasValue()) {
                entry = _jsonProvider.DeserializeObject<SerpEntry>(json);
            } else {
                entry = new SerpEntry();
            }

            return entry;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(SerpEntry);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }
    }
}
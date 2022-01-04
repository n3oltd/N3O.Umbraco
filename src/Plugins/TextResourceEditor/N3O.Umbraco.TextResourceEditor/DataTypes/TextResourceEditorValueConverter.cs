using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;
using N3O.Umbraco.ValueConverters;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.TextResourceEditor.DataTypes {
    public class TemplateTextEditorValueConverter : PropertyValueConverter {
        private readonly IJsonProvider _jsonProvider;

        public TemplateTextEditorValueConverter(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
        
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.EqualsInvariant(TextResourceEditorConstants.PropertyEditorAlias);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                           IPublishedPropertyType propertyType,
                                                           object source,
                                                           bool preview) {
            var textResources = new List<TextResource>();

            if (source is string json && json.HasValue()) {
                textResources.AddRange(_jsonProvider.DeserializeObject<IEnumerable<TextResource>>(json));
            }

            return textResources;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(IEnumerable<TextResource>);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }
    }
}
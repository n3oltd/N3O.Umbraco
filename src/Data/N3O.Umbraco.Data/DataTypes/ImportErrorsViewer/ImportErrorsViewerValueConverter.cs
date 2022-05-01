using N3O.Umbraco.Extensions;
using N3O.Umbraco.ValueConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportErrorsViewerValueConverter : PropertyValueConverter {
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.EqualsInvariant(DataConstants.PropertyEditorAliases.ImportErrorsViewer);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                           IPublishedPropertyType propertyType,
                                                           object source,
                                                           bool preview) {
            var errors = new List<string>();

            if (source is string json && json.HasValue()) {
                errors.AddRange(JsonConvert.DeserializeObject<IEnumerable<string>>(json));
            }

            return errors;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(IEnumerable<string>);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }
    }
}
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ValueConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportFieldsEditorValueConverter : PropertyValueConverter {
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.EqualsInvariant(DataConstants.PropertyEditorAliases.ImportFieldsEditor);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                           IPublishedPropertyType propertyType,
                                                           object source,
                                                           bool preview) {
            var importFields = new List<ImportField>();

            if (source is string json && json.HasValue()) {
                importFields.AddRange(JsonConvert.DeserializeObject<IEnumerable<ImportField>>(json));
            }

            return importFields;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(IEnumerable<ImportField>);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }
    }
}
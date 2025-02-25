using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.ValueConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes;

public class ImportDataEditorValueConverter : PropertyValueConverter {
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.EqualsInvariant(DataConstants.PropertyEditorAliases.ImportDataEditor);
    }

    public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                       IPublishedPropertyType propertyType,
                                                       object source,
                                                       bool preview) {
        var importData = default(ImportData);
        var json = default(string);
        
        if (source is string str) {
            json = str;
        } else if (source is JObject jObject) {
            json = jObject.ToString();
        }

        if (json.HasValue()) {
            importData = JsonConvert.DeserializeObject<ImportData>(json);
        }

        return importData;
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
        return typeof(ImportData);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
        return PropertyCacheLevel.Element;
    }
}

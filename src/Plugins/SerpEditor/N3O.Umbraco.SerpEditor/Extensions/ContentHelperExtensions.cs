using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.SerpEditor.DataTypes;
using N3O.Umbraco.SerpEditor.Models;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.SerpEditor.Extensions;

public static class ContentHelperExtensions {
    public static SerpEntry GetSerpEntry(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.HasEditorAlias(SerpEditorConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not a SERP Editor");
        }

        return GetSerpEntry(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }

    public static SerpEntry GetSerpEntry(this IContentHelper contentHelper,
                                         string contentTypeAlias,
                                         IProperty property) {
        if (!property.PropertyType.HasEditorAlias(SerpEditorConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not a SERP Editor");
        }

        return GetSerpEntry(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static SerpEntry GetSerpEntry(this IContentHelper contentHelper,
                                         string contentTypeAlias,
                                         string propertyTypeAlias,
                                         object propertyValue) {
        if (propertyValue == null) {
            return null;
        }

        var serpEntry = contentHelper.GetConvertedValue<SerpEditorValueConverter, SerpEntry>(contentTypeAlias,
                                                                                             propertyTypeAlias,
                                                                                             propertyValue);

        return serpEntry;
    }
}
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Uploader.DataTypes;
using N3O.Umbraco.Uploader.Models;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Uploader.Extensions;

public static class ContentHelperExtensions {
    public static FileUpload GetFileUpload(this IContentHelper contentHelper,
                                           ContentProperties contentProperties,
                                           string propertyTypeAlias) {
        var contentProperty = contentProperties.GetPropertyByAlias(propertyTypeAlias);
        
        return GetFileUpload(contentHelper, contentProperty);
    }
    
    public static FileUpload GetFileUpload(this IContentHelper contentHelper, IContentProperty property) {
        if (!property.Type.HasEditorAlias(UploaderConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not uploader");
        }
        
        return GetFileUpload(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
    }
    
    public static FileUpload GetFileUpload(this IContentHelper contentHelper,
                                           string contentTypeAlias,
                                           IProperty property) {
        if (!property.PropertyType.HasEditorAlias(UploaderConstants.PropertyEditorAlias)) {
            throw new Exception("Property is not uploader");
        }
        
        return GetFileUpload(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
    }

    public static FileUpload GetFileUpload(this IContentHelper contentHelper,
                                           string contentTypeAlias,
                                           string propertyTypeAlias,
                                           object propertyValue) {
        if (propertyValue == null) {
            return null;
        }

        var fileUpload = contentHelper.GetConvertedValue<UploaderValueConverter, FileUpload>(contentTypeAlias,
                                                                                             propertyTypeAlias,
                                                                                             propertyValue);

        return fileUpload;
    }
}

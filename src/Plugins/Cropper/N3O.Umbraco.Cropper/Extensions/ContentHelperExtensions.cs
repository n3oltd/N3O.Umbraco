using N3O.Umbraco.Content;
using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cropper.Extensions {
    public static class ContentHelperExtensions {
        public static CroppedImage GetCroppedImage(this IContentHelper contentHelper, IContentProperty property) {
            if (!property.Type.HasEditorAlias(CropperConstants.PropertyEditorAlias)) {
                throw new Exception("Property is not image cropper");
            }
            
            return GetCroppedImage(contentHelper, property.ContentType.Alias, property.Type.Alias, property.Value);
        }
        
        public static CroppedImage GetCroppedImage(this IContentHelper contentHelper,
                                                   string contentTypeAlias,
                                                   IProperty property) {
            if (!property.PropertyType.HasEditorAlias(CropperConstants.PropertyEditorAlias)) {
                throw new Exception("Property is not image cropper");
            }
            
            return GetCroppedImage(contentHelper, contentTypeAlias, property.PropertyType.Alias, property.GetValue());
        }
        
        public static CroppedImage GetCroppedImage(this IContentHelper contentHelper,
                                                   string contentTypeAlias,
                                                   string propertyTypeAlias,
                                                   object propertyValue) {
            if (propertyValue == null) {
                return null;
            }

            var croppedImage = contentHelper.GetConvertedValue<CropperValueConverter, CroppedImage>(contentTypeAlias,
                                                                                                    propertyTypeAlias,
                                                                                                    propertyValue);

            return croppedImage;
        }
    }
}
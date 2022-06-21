using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cropper.DataTypes;

public class CropperValueConverter : PropertyValueConverterBase {
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias.EqualsInvariant(CropperConstants.PropertyEditorAlias);
    }

    public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                       IPublishedPropertyType propertyType,
                                                       object source,
                                                       bool preview) {
        CroppedImage croppedImage = null;

        if (source is string json && json.HasValue()) {
            var configuration = propertyType.DataType.ConfigurationAs<CropperConfiguration>();
            var cropperSource = JsonConvert.DeserializeObject<CropperSource>(json);
        
            croppedImage = new CroppedImage(configuration, cropperSource);
        }

        return croppedImage;
    }

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
        return typeof(CroppedImage);
    }

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
        return PropertyCacheLevel.Element;
    }
}

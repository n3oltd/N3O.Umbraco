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
        var json = source as string ?? source.IfNotNull(JsonConvert.SerializeObject);

        if (json.HasValue()) {
            var configuration = propertyType.DataType.ConfigurationAs<CropperConfiguration>();
            var cropperSource = JsonConvert.DeserializeObject<CropperSource>(json);
        
            croppedImage = cropperSource.IfNotNull(x => new CroppedImage(configuration, x));
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

using N3O.Umbraco.Extensions;
using N3O.Umbraco.Uploader.Models;
using Newtonsoft.Json;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes {
    public class UploaderValueConverter : PropertyValueConverterBase {
        public override bool IsConverter(IPublishedPropertyType propertyType) {
            return propertyType.EditorAlias.EqualsInvariant(UploaderConstants.PropertyEditorAlias);
        }

        public override object ConvertSourceToIntermediate(IPublishedElement owner,
                                                           IPublishedPropertyType propertyType,
                                                           object source,
                                                           bool preview) {
            FileUpload fileUpload = null;

            if (source is string json && json.HasValue()) {
                var uploaderSource = JsonConvert.DeserializeObject<UploaderSource>(json);
            
                fileUpload = new FileUpload(uploaderSource);
            }

            return fileUpload;
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {
            return typeof(FileUpload);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) {
            return PropertyCacheLevel.Element;
        }
    }
}

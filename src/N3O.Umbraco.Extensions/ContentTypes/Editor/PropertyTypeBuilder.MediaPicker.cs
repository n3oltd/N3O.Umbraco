using N3O.Umbraco.DataTypes;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class MediaPickerPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<MediaPickerPropertyTypeBuilder, MediaPickerDataTypeDesigner> {
    public MediaPickerPropertyTypeBuilder(IDataTypeService dataTypeService,
                                          MediaPickerDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        if (HasConfiguration) {
            return BuildInlineDataType(context);
        } else {
            var key = Guid.Parse(global::Umbraco.Cms.Core.Constants.DataTypes.Guids.MediaPicker3);

            return DataTypeService.GetDataType(key);
        }
    }
}

using N3O.Umbraco.DataTypes;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class ColorPickerPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<ColorPickerPropertyTypeBuilder, ColorPickerDataTypeDesigner> {
    public ColorPickerPropertyTypeBuilder(IDataTypeService dataTypeService,
                                          ColorPickerDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        if (HasConfiguration) {
            return BuildInlineDataType(context);
        } else {
            var key = Guid.Parse(global::Umbraco.Cms.Core.Constants.DataTypes.Guids.ApprovedColor);

            return DataTypeService.GetDataType(key);
        }
    }
}

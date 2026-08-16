using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Cropper.DataTypes;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cropper.ContentTypes;

public class CropperPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<CropperPropertyTypeBuilder, CropperDataTypeDesigner> {
    public CropperPropertyTypeBuilder(IDataTypeService dataTypeService, CropperDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return BuildInlineDataType(context);
    }
}

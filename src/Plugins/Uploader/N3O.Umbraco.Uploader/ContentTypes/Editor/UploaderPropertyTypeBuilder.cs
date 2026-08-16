using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Uploader.DataTypes;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Uploader.ContentTypes;

public class UploaderPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<UploaderPropertyTypeBuilder, UploaderDataTypeDesigner> {
    public UploaderPropertyTypeBuilder(IDataTypeService dataTypeService, UploaderDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return BuildInlineDataType(context);
    }
}

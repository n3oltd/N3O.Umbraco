using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class ExistingDataTypePropertyTypeBuilder : PropertyTypeBuilder<ExistingDataTypePropertyTypeBuilder> {
    public ExistingDataTypePropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    // No default: the editor has no designer in this library, so the site owns the data type and callers
    // must point at it with DataType(nameOrKey). ResolveDataType throws if they do not.
    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return null;
    }
}

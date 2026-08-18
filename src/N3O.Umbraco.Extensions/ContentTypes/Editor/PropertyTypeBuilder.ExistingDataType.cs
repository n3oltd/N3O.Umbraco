using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class ExistingDataTypePropertyTypeBuilder : PropertyTypeBuilder<ExistingDataTypePropertyTypeBuilder> {
    public ExistingDataTypePropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    // No default: callers must point at the site's own data type with DataType(nameOrKey).
    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return null;
    }
}

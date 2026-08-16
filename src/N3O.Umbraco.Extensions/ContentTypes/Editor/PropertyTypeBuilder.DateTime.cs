using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class DateTimePropertyTypeBuilder : PropertyTypeBuilder<DateTimePropertyTypeBuilder> {
    public DateTimePropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return DataTypeService.GetDataType(global::Umbraco.Cms.Core.Constants.DataTypes.DateTime);
    }
}

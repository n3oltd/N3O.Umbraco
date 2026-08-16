using N3O.Umbraco.DataTypes;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class DateTimePropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<DateTimePropertyTypeBuilder, DateTimeDataTypeDesigner> {
    public DateTimePropertyTypeBuilder(IDataTypeService dataTypeService, DateTimeDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        if (HasConfiguration) {
            return BuildInlineDataType(context);
        } else {
            return DataTypeService.GetDataType(global::Umbraco.Cms.Core.Constants.DataTypes.DateTime);
        }
    }
}

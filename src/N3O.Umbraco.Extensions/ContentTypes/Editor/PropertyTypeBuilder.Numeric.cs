using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class NumericPropertyTypeBuilder : PropertyTypeBuilder<NumericPropertyTypeBuilder> {
    public NumericPropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        var key = Guid.Parse(global::Umbraco.Cms.Core.Constants.DataTypes.Guids.Numeric);

        return DataTypeService.GetDataType(key);
    }
}

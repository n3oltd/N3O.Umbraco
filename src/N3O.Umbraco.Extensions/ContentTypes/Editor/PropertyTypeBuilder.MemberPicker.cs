using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class MemberPickerPropertyTypeBuilder : PropertyTypeBuilder<MemberPickerPropertyTypeBuilder> {
    public MemberPickerPropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        var key = Guid.Parse(global::Umbraco.Cms.Core.Constants.DataTypes.Guids.MemberPicker);

        return DataTypeService.GetDataType(key);
    }
}

using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class TextBoxPropertyTypeBuilder : PropertyTypeBuilder<TextBoxPropertyTypeBuilder> {
    public TextBoxPropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return DataTypeService.GetDataType(global::Umbraco.Cms.Core.Constants.DataTypes.Textbox);
    }
}

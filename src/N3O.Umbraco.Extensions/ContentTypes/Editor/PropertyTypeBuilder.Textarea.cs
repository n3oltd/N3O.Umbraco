using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class TextareaPropertyTypeBuilder : PropertyTypeBuilder<TextareaPropertyTypeBuilder> {
    public TextareaPropertyTypeBuilder(IDataTypeService dataTypeService) : base(dataTypeService) { }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return DataTypeService.GetDataType(global::Umbraco.Cms.Core.Constants.DataTypes.Textarea);
    }
}

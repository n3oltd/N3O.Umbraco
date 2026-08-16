using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class TextareaDataTypeDesigner : DataTypeDesigner {
    private int? _maxChars;
    private int? _rows;

    public TextareaDataTypeDesigner(IDataTypeService dataTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public TextareaDataTypeDesigner MaxChars(int maxChars) {
        _maxChars = maxChars;

        return this;
    }

    public TextareaDataTypeDesigner Rows(int rows) {
        _rows = rows;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new TextAreaConfiguration();

        configuration.MaxChars = _maxChars;
        configuration.Rows = _rows;

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.TextArea;
}

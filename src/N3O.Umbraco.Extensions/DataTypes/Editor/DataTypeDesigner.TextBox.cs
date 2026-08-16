using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class TextBoxDataTypeDesigner : DataTypeDesigner {
    private int? _maxChars;

    public TextBoxDataTypeDesigner(IDataTypeService dataTypeService,
                                   PropertyEditorCollection propertyEditors,
                                   IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public TextBoxDataTypeDesigner MaxChars(int maxChars) {
        _maxChars = maxChars;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new TextboxConfiguration();

        configuration.MaxChars = _maxChars;

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.TextBox;
}

using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class ContentPickerDataTypeDesigner : DataTypeDesigner {
    private bool _showOpenButton;

    public ContentPickerDataTypeDesigner(IDataTypeService dataTypeService,
                                         PropertyEditorCollection propertyEditors,
                                         IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public ContentPickerDataTypeDesigner ShowOpenButton() {
        _showOpenButton = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new ContentPickerConfiguration();

        configuration.ShowOpenButton = _showOpenButton;

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.ContentPicker;
}

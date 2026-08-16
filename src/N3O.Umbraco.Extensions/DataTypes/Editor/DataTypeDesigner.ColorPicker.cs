using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class ColorPickerDataTypeDesigner : ValueListDataTypeDesigner<ColorPickerDataTypeDesigner> {
    private bool _useLabel;

    public ColorPickerDataTypeDesigner(IDataTypeService dataTypeService,
                                       PropertyEditorCollection propertyEditors,
                                       IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public ColorPickerDataTypeDesigner UseLabel() {
        _useLabel = true;

        return this;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.ColorPicker;

    protected override ValueListConfiguration CreateConfiguration() {
        var configuration = new ColorPickerConfiguration();

        configuration.UseLabel = _useLabel;

        return configuration;
    }
}

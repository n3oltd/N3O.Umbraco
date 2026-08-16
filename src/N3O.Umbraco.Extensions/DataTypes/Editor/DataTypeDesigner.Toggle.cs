using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class ToggleDataTypeDesigner : DataTypeDesigner {
    private bool _default;
    private string _labelOff;
    private string _labelOn;
    private bool _showLabels;

    public ToggleDataTypeDesigner(IDataTypeService dataTypeService,
                                  PropertyEditorCollection propertyEditors,
                                  IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public ToggleDataTypeDesigner DefaultOn() {
        _default = true;

        return this;
    }

    public ToggleDataTypeDesigner Labels(string labelOn, string labelOff) {
        _labelOn = labelOn;
        _labelOff = labelOff;
        _showLabels = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new TrueFalseConfiguration();

        configuration.Default = _default;
        configuration.ShowLabels = _showLabels;

        if (_labelOn.HasValue()) {
            configuration.LabelOn = _labelOn;
        }

        if (_labelOff.HasValue()) {
            configuration.LabelOff = _labelOff;
        }

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.Boolean;
}

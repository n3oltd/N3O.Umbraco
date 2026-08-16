using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class ColorPickerDataTypeDesigner : ValueListDataTypeDesigner<ColorPickerDataTypeDesigner> {
    private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
    private readonly Dictionary<string, string> _labels = new(StringComparer.InvariantCultureIgnoreCase);

    private bool _useLabel;

    public ColorPickerDataTypeDesigner(IDataTypeService dataTypeService,
                                       PropertyEditorCollection propertyEditors,
                                       IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) {
        _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
    }

    public ColorPickerDataTypeDesigner AddColor(string color, string label) {
        _labels[ToHex(color)] = label;
        _useLabel = true;

        return Options(color);
    }

    public ColorPickerDataTypeDesigner UseLabel() {
        _useLabel = true;

        return this;
    }

    // Umbraco validates colours as bare three or six digit hex, and pairs them with labels as a JSON value
    protected override string BuildItemValue(string value) {
        var color = ToHex(value);

        if (!_useLabel) {
            return color;
        }

        if (!_labels.TryGetValue(color, out var label)) {
            label = color;
        }

        return _configurationEditorJsonSerializer.Serialize(new { value = color, label });
    }

    protected override ValueListConfiguration CreateConfiguration() {
        var configuration = new ColorPickerConfiguration();

        configuration.UseLabel = _useLabel;

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.ColorPicker;

    private string ToHex(string color) {
        return color.TrimStart('#');
    }
}

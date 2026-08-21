using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class EyeDropperDataTypeDesigner : DataTypeDesigner {
    private bool _showAlpha;
    private bool _showPalette;

    public EyeDropperDataTypeDesigner(IDataTypeService dataTypeService,
                                      PropertyEditorCollection propertyEditors,
                                      IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public EyeDropperDataTypeDesigner ShowAlpha() {
        _showAlpha = true;

        return this;
    }

    public EyeDropperDataTypeDesigner ShowPalette() {
        _showPalette = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new EyeDropperColorPickerConfiguration();

        configuration.ShowAlpha = _showAlpha;
        configuration.ShowPalette = _showPalette;

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.ColorPickerEyeDropper;
}

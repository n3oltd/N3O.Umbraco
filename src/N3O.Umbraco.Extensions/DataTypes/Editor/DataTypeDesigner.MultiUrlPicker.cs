using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class MultiUrlPickerDataTypeDesigner : DataTypeDesigner {
    private bool _hideAnchor;
    private int _maxNumber;
    private int _minNumber;

    public MultiUrlPickerDataTypeDesigner(IDataTypeService dataTypeService,
                                          PropertyEditorCollection propertyEditors,
                                          IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public MultiUrlPickerDataTypeDesigner HideAnchor() {
        _hideAnchor = true;

        return this;
    }

    public MultiUrlPickerDataTypeDesigner Limit(int min, int max) {
        _minNumber = min;
        _maxNumber = max;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new MultiUrlPickerConfiguration();

        configuration.HideAnchor = _hideAnchor;
        configuration.MaxNumber = _maxNumber;
        configuration.MinNumber = _minNumber;

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultiUrlPicker;
}

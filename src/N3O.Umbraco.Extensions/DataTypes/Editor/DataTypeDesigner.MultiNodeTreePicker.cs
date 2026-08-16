using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class MultiNodeTreePickerDataTypeDesigner : DataTypeDesigner {
    private string _filter;
    private int _maxNumber;
    private int _minNumber;
    private bool _showOpen;

    public MultiNodeTreePickerDataTypeDesigner(IDataTypeService dataTypeService,
                                               PropertyEditorCollection propertyEditors,
                                               IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public MultiNodeTreePickerDataTypeDesigner AllowContentTypes(params string[] contentTypeAliases) {
        _filter = contentTypeAliases.ToCsv();

        return this;
    }

    public MultiNodeTreePickerDataTypeDesigner Limit(int min, int max) {
        _minNumber = min;
        _maxNumber = max;

        return this;
    }

    public MultiNodeTreePickerDataTypeDesigner ShowOpen() {
        _showOpen = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new MultiNodePickerConfiguration();

        configuration.MaxNumber = _maxNumber;
        configuration.MinNumber = _minNumber;
        configuration.ShowOpen = _showOpen;

        if (_filter.HasValue()) {
            configuration.Filter = _filter;
        }

        return configuration;
    }

    protected override string EditorAlias =>
        global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultiNodeTreePicker;
}

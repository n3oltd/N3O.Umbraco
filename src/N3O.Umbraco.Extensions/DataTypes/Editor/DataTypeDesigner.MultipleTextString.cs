using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class MultipleTextStringDataTypeDesigner : DataTypeDesigner {
    private int _maximum;
    private int _minimum;

    public MultipleTextStringDataTypeDesigner(IDataTypeService dataTypeService,
                                              PropertyEditorCollection propertyEditors,
                                              IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public MultipleTextStringDataTypeDesigner Limit(int min, int max) {
        _minimum = min;
        _maximum = max;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new MultipleTextStringConfiguration();

        configuration.Maximum = _maximum;
        configuration.Minimum = _minimum;

        return configuration;
    }

    protected override string EditorAlias =>
        global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.MultipleTextstring;
}

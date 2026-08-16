using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class SliderDataTypeDesigner : DataTypeDesigner {
    private bool _enableRange;
    private decimal _initialValue;
    private decimal _maximumValue;
    private decimal _minimumValue;
    private decimal _stepIncrements = 1;

    public SliderDataTypeDesigner(IDataTypeService dataTypeService,
                                  PropertyEditorCollection propertyEditors,
                                  IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public SliderDataTypeDesigner EnableRange() {
        _enableRange = true;

        return this;
    }

    public SliderDataTypeDesigner InitialValue(decimal initialValue) {
        _initialValue = initialValue;

        return this;
    }

    public SliderDataTypeDesigner Range(decimal min, decimal max) {
        _minimumValue = min;
        _maximumValue = max;

        return this;
    }

    public SliderDataTypeDesigner StepIncrements(decimal stepIncrements) {
        _stepIncrements = stepIncrements;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new SliderConfiguration();

        configuration.EnableRange = _enableRange;
        configuration.InitialValue = _initialValue;
        configuration.MaximumValue = _maximumValue;
        configuration.MinimumValue = _minimumValue;
        configuration.StepIncrements = _stepIncrements;

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.Slider;
}

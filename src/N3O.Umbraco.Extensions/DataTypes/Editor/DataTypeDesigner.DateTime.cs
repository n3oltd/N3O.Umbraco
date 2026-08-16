using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class DateTimeDataTypeDesigner : DataTypeDesigner {
    private string _format = "YYYY-MM-DD HH:mm:ss";
    private bool _offsetTime;

    public DateTimeDataTypeDesigner(IDataTypeService dataTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public DateTimeDataTypeDesigner Format(string format) {
        _format = format;

        return this;
    }

    public DateTimeDataTypeDesigner OffsetTime() {
        _offsetTime = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new DateTimeConfiguration();

        configuration.Format = _format;
        configuration.OffsetTime = _offsetTime;

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.DateTime;
}

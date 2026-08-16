using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public abstract class ValueListDataTypeDesigner<TSelf> : DataTypeDesigner
    where TSelf : ValueListDataTypeDesigner<TSelf> {
    private readonly List<string> _options = [];

    protected ValueListDataTypeDesigner(IDataTypeService dataTypeService,
                                        PropertyEditorCollection propertyEditors,
                                        IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public TSelf Options(params string[] values) {
        _options.AddRange(values);

        return (TSelf) this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = CreateConfiguration();

        configuration.Items = _options.Select((value, index) => {
                                          var item = new ValueListConfiguration.ValueListItem();

                                          item.Id = index + 1;
                                          item.Value = BuildItemValue(value);

                                          return item;
                                      })
                                      .ToList();

        return configuration;
    }

    protected virtual string BuildItemValue(string value) {
        return value;
    }

    protected virtual ValueListConfiguration CreateConfiguration() {
        return new ValueListConfiguration();
    }
}

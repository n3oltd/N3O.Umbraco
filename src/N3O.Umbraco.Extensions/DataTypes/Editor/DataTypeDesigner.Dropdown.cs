using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class DropdownDataTypeDesigner : DataTypeDesigner {
    private readonly List<string> _options = [];

    private bool _multiple;

    public DropdownDataTypeDesigner(IDataTypeService dataTypeService,
                                    PropertyEditorCollection propertyEditors,
                                    IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) { }

    public DropdownDataTypeDesigner AllowMultiple() {
        _multiple = true;

        return this;
    }

    public DropdownDataTypeDesigner Options(params string[] values) {
        _options.AddRange(values);

        return this;
    }

    protected override object BuildConfiguration() {
        var configuration = new DropDownFlexibleConfiguration();

        configuration.Multiple = _multiple;
        configuration.Items = _options.Select((value, index) => {
                                          var item = new ValueListConfiguration.ValueListItem();

                                          item.Id = index + 1;
                                          item.Value = value;

                                          return item;
                                      })
                                      .ToList();

        return configuration;
    }

    protected override string EditorAlias =>
        global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.DropDownListFlexible;
}

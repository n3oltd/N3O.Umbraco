using N3O.Umbraco.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class DropdownPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<DropdownPropertyTypeBuilder, DropdownDataTypeDesigner> {
    private readonly List<string> _options = [];

    private bool _multiple;

    public DropdownPropertyTypeBuilder(IDataTypeService dataTypeService, DropdownDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    public DropdownPropertyTypeBuilder AllowMultiple() {
        _multiple = true;

        return this;
    }

    public DropdownPropertyTypeBuilder Options(params string[] values) {
        _options.AddRange(values);

        return this;
    }

    protected override void ConfigureDataType(DropdownDataTypeDesigner dataTypeDesigner) {
        dataTypeDesigner.Options(_options.ToArray());

        if (_multiple) {
            dataTypeDesigner.AllowMultiple();
        }
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return BuildInlineDataType(context);
    }
}

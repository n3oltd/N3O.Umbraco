using Humanizer;
using N3O.Umbraco.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class DropdownPropertyTypeBuilder : PropertyTypeBuilder<DropdownPropertyTypeBuilder> {
    private readonly DropdownDataTypeDesigner _dataTypeDesigner;
    private readonly List<string> _options = [];

    private bool _multiple;

    public DropdownPropertyTypeBuilder(IDataTypeService dataTypeService, DropdownDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService) {
        _dataTypeDesigner = dataTypeDesigner;
    }

    public DropdownPropertyTypeBuilder AllowMultiple() {
        _multiple = true;

        return this;
    }

    public DropdownPropertyTypeBuilder Options(params string[] values) {
        _options.AddRange(values);

        return this;
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        _dataTypeDesigner.SetName($"{context.ContentTypeAlias.Titleize()} {context.PropertyAlias.Titleize()}");
        _dataTypeDesigner.WithoutNameAdoption();
        _dataTypeDesigner.Options(_options.ToArray());

        if (_multiple) {
            _dataTypeDesigner.AllowMultiple();
        }

        if (context.UseDeterministicIds) {
            _dataTypeDesigner.WithDeterministicId($"{context.ContentTypeAlias}_{context.PropertyAlias}");
        }

        return _dataTypeDesigner.Save();
    }
}

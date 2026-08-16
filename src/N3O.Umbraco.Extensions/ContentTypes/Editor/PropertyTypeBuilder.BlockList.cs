using Humanizer;
using N3O.Umbraco.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class BlockListPropertyTypeBuilder : PropertyTypeBuilder<BlockListPropertyTypeBuilder> {
    private readonly BlockListDataTypeDesigner _dataTypeDesigner;
    private readonly List<string> _elementTypeAliases = [];

    public BlockListPropertyTypeBuilder(IDataTypeService dataTypeService, BlockListDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService) {
        _dataTypeDesigner = dataTypeDesigner;
    }

    public BlockListPropertyTypeBuilder AllowBlocks(params string[] elementTypeAliases) {
        _elementTypeAliases.AddRange(elementTypeAliases);

        return this;
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        _dataTypeDesigner.SetName($"{context.ContentTypeAlias.Titleize()} {context.PropertyAlias.Titleize()}");
        _dataTypeDesigner.WithoutNameAdoption();
        _dataTypeDesigner.AllowBlocks(_elementTypeAliases.ToArray());

        if (context.UseDeterministicIds) {
            _dataTypeDesigner.WithDeterministicId($"{context.ContentTypeAlias}_{context.PropertyAlias}");
        }

        return _dataTypeDesigner.Save();
    }
}

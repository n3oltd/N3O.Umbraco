using N3O.Umbraco.DataTypes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.ContentTypes;

public class BlockListPropertyTypeBuilder
    : ConfiguredPropertyTypeBuilder<BlockListPropertyTypeBuilder, BlockListDataTypeDesigner> {
    private readonly List<string> _elementTypeAliases = [];

    public BlockListPropertyTypeBuilder(IDataTypeService dataTypeService, BlockListDataTypeDesigner dataTypeDesigner)
        : base(dataTypeService, dataTypeDesigner) { }

    public BlockListPropertyTypeBuilder AllowBlocks(params string[] elementTypeAliases) {
        _elementTypeAliases.AddRange(elementTypeAliases);

        return this;
    }

    protected override void ConfigureDataType(BlockListDataTypeDesigner dataTypeDesigner) {
        dataTypeDesigner.AllowBlocks(_elementTypeAliases.ToArray());
    }

    protected override IDataType GetDefaultDataType(PropertyTypeContext context) {
        return BuildInlineDataType(context);
    }
}

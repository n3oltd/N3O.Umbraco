using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class BlockListDataTypeDesigner : DataTypeDesigner {
    private readonly IContentTypeService _contentTypeService;
    private readonly List<string> _elementTypeAliases = [];

    public BlockListDataTypeDesigner(IDataTypeService dataTypeService,
                                     IContentTypeService contentTypeService,
                                     PropertyEditorCollection propertyEditors,
                                     IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) {
        _contentTypeService = contentTypeService;
    }

    public BlockListDataTypeDesigner AllowBlocks(params string[] elementTypeAliases) {
        _elementTypeAliases.AddRange(elementTypeAliases);

        return this;
    }

    protected override object BuildConfiguration() {
        var blocks = new List<BlockListConfiguration.BlockConfiguration>();

        foreach (var alias in _elementTypeAliases) {
            var elementType = _contentTypeService.Get(alias);

            if (elementType == null) {
                throw new Exception($"No element type found with alias {alias.Quote()}");
            }

            var block = new BlockListConfiguration.BlockConfiguration();

            block.ContentElementTypeKey = elementType.Key;

            blocks.Add(block);
        }

        var configuration = new BlockListConfiguration();

        configuration.Blocks = blocks.ToArray();

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.BlockList;
}

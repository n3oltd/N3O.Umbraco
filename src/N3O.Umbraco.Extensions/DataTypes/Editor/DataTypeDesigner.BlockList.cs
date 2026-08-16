using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.DataTypes;

public class BlockListDataTypeDesigner : DataTypeDesigner {
    private readonly IContentTypeService _contentTypeService;
    private readonly List<BlockListBlockBuilder> _blocks = [];

    private int? _maxBlocks;
    private string _maxPropertyWidth;
    private int? _minBlocks;
    private bool _useInlineEditingAsDefault;
    private bool _useLiveEditing;
    private bool _useSingleBlockMode;

    public BlockListDataTypeDesigner(IDataTypeService dataTypeService,
                                     IContentTypeService contentTypeService,
                                     PropertyEditorCollection propertyEditors,
                                     IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) {
        _contentTypeService = contentTypeService;
    }

    public BlockListBlockBuilder AddBlock(string elementTypeAlias) {
        var block = new BlockListBlockBuilder(elementTypeAlias);

        _blocks.Add(block);

        return block;
    }

    public BlockListDataTypeDesigner AllowBlocks(params string[] elementTypeAliases) {
        foreach (var alias in elementTypeAliases) {
            AddBlock(alias);
        }

        return this;
    }

    public BlockListDataTypeDesigner Limit(int? min, int? max) {
        _minBlocks = min;
        _maxBlocks = max;

        return this;
    }

    public BlockListDataTypeDesigner SetMaxPropertyWidth(string maxPropertyWidth) {
        _maxPropertyWidth = maxPropertyWidth;

        return this;
    }

    public BlockListDataTypeDesigner UseInlineEditingAsDefault() {
        _useInlineEditingAsDefault = true;

        return this;
    }

    public BlockListDataTypeDesigner UseLiveEditing() {
        _useLiveEditing = true;

        return this;
    }

    public BlockListDataTypeDesigner UseSingleBlockMode() {
        _useSingleBlockMode = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var configuration = new BlockListConfiguration();

        configuration.Blocks = _blocks.Select(x => x.Build(ResolveElementType)).ToArray();
        configuration.UseInlineEditingAsDefault = _useInlineEditingAsDefault;
        configuration.UseLiveEditing = _useLiveEditing;
        configuration.UseSingleBlockMode = _useSingleBlockMode;

        if (_minBlocks.HasValue() || _maxBlocks.HasValue()) {
            configuration.ValidationLimit.Min = _minBlocks;
            configuration.ValidationLimit.Max = _maxBlocks;
        }

        if (_maxPropertyWidth.HasValue()) {
            configuration.MaxPropertyWidth = _maxPropertyWidth;
        }

        return configuration;
    }

    protected override string EditorAlias => global::Umbraco.Cms.Core.Constants.PropertyEditors.Aliases.BlockList;

    private IContentType ResolveElementType(string alias) {
        var elementType = _contentTypeService.Get(alias);

        if (elementType == null) {
            throw new Exception($"No element type found with alias {alias.Quote()}");
        }

        return elementType;
    }
}

public class BlockListBlockBuilder {
    private readonly string _elementTypeAlias;

    private string _backgroundColor;
    private string _editorSize;
    private bool _hideContentEditorInOverlay;
    private string _iconColor;
    private string _label;
    private string _settingsElementTypeAlias;
    private string _thumbnail;

    public BlockListBlockBuilder(string elementTypeAlias) {
        _elementTypeAlias = elementTypeAlias;
    }

    public BlockListBlockBuilder BackgroundColor(string backgroundColor) {
        _backgroundColor = backgroundColor;

        return this;
    }

    public BlockListBlockBuilder EditorSize(string editorSize) {
        _editorSize = editorSize;

        return this;
    }

    public BlockListBlockBuilder HideContentEditorInOverlay() {
        _hideContentEditorInOverlay = true;

        return this;
    }

    public BlockListBlockBuilder IconColor(string iconColor) {
        _iconColor = iconColor;

        return this;
    }

    public BlockListBlockBuilder Label(string label) {
        _label = label;

        return this;
    }

    public BlockListBlockBuilder Thumbnail(string thumbnail) {
        _thumbnail = thumbnail;

        return this;
    }

    public BlockListBlockBuilder WithSettings(string settingsElementTypeAlias) {
        _settingsElementTypeAlias = settingsElementTypeAlias;

        return this;
    }

    public BlockListConfiguration.BlockConfiguration Build(Func<string, IContentType> resolveElementType) {
        var block = new BlockListConfiguration.BlockConfiguration();

        block.ContentElementTypeKey = resolveElementType(_elementTypeAlias).Key;
        block.BackgroundColor = _backgroundColor;
        block.EditorSize = _editorSize;
        block.ForceHideContentEditorInOverlay = _hideContentEditorInOverlay;
        block.IconColor = _iconColor;
        block.Label = _label;
        block.Thumbnail = _thumbnail;

        if (_settingsElementTypeAlias.HasValue()) {
            block.SettingsElementTypeKey = resolveElementType(_settingsElementTypeAlias).Key;
        }

        return block;
    }
}

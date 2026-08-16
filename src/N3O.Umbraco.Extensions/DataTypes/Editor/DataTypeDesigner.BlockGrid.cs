using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.DataTypes;

public class BlockGridDataTypeDesigner : DataTypeDesigner {
    private readonly IContentTypeService _contentTypeService;
    private readonly List<BlockGridBlockBuilder> _blocks = [];
    private readonly List<string> _groups = [];

    private string _createLabel;
    private int? _gridColumns;
    private string _layoutStylesheet;
    private int? _maxBlocks;
    private string _maxPropertyWidth;
    private int? _minBlocks;
    private bool _useLiveEditing;

    public BlockGridDataTypeDesigner(IDataTypeService dataTypeService,
                                     IContentTypeService contentTypeService,
                                     PropertyEditorCollection propertyEditors,
                                     IConfigurationEditorJsonSerializer configurationEditorJsonSerializer)
        : base(dataTypeService, propertyEditors, configurationEditorJsonSerializer) {
        _contentTypeService = contentTypeService;
    }

    public BlockGridBlockBuilder AddBlock(string elementTypeAlias) {
        var block = new BlockGridBlockBuilder(elementTypeAlias);

        _blocks.Add(block);

        return block;
    }

    public BlockGridDataTypeDesigner AddGroup(string name) {
        _groups.Add(name);

        return this;
    }

    public BlockGridDataTypeDesigner Limit(int? min, int? max) {
        _minBlocks = min;
        _maxBlocks = max;

        return this;
    }

    public BlockGridDataTypeDesigner SetCreateLabel(string createLabel) {
        _createLabel = createLabel;

        return this;
    }

    public BlockGridDataTypeDesigner SetGridColumns(int gridColumns) {
        _gridColumns = gridColumns;

        return this;
    }

    public BlockGridDataTypeDesigner SetLayoutStylesheet(string layoutStylesheet) {
        _layoutStylesheet = layoutStylesheet;

        return this;
    }

    public BlockGridDataTypeDesigner SetMaxPropertyWidth(string maxPropertyWidth) {
        _maxPropertyWidth = maxPropertyWidth;

        return this;
    }

    public BlockGridDataTypeDesigner UseLiveEditing() {
        _useLiveEditing = true;

        return this;
    }

    protected override object BuildConfiguration(IDataType existing) {
        var existingConfiguration = existing?.Configuration as BlockGridConfiguration;
        var configuration = new BlockGridConfiguration();

        configuration.BlockGroups = _groups.Select(name => BuildGroup(name, existingConfiguration)).ToArray();
        configuration.Blocks = _blocks.Select(x => BuildBlock(x, configuration, existingConfiguration)).ToArray();
        configuration.UseLiveEditing = _useLiveEditing;

        if (_minBlocks.HasValue() || _maxBlocks.HasValue()) {
            configuration.ValidationLimit.Min = _minBlocks;
            configuration.ValidationLimit.Max = _maxBlocks;
        }

        if (_gridColumns.HasValue()) {
            configuration.GridColumns = _gridColumns;
        }

        if (_createLabel.HasValue()) {
            configuration.CreateLabel = _createLabel;
        }

        if (_layoutStylesheet.HasValue()) {
            configuration.LayoutStylesheet = _layoutStylesheet;
        }

        if (_maxPropertyWidth.HasValue()) {
            configuration.MaxPropertyWidth = _maxPropertyWidth;
        }

        return configuration;
    }

    protected override string EditorAlias => UmbracoPropertyEditors.Aliases.BlockGrid;

    private BlockGridConfiguration.BlockGridBlockConfiguration BuildBlock(BlockGridBlockBuilder block,
                                                                          BlockGridConfiguration configuration,
                                                                          BlockGridConfiguration existing) {
        return block.Build(ResolveElementType,
                           groupName => FindGroupKey(configuration, groupName),
                           areaAlias => GetAreaKey(block.ElementTypeAlias, areaAlias, existing));
    }

    private BlockGridConfiguration.BlockGridGroupConfiguration BuildGroup(string name,
                                                                          BlockGridConfiguration existing) {
        var group = new BlockGridConfiguration.BlockGridGroupConfiguration();

        group.Key = existing?.BlockGroups.FirstOrDefault(x => x.Name.EqualsInvariant(name))?.Key ??
                    UmbracoId.Deterministic(IdScope.BlockGroup, name);
        group.Name = name;

        return group;
    }

    private Guid FindGroupKey(BlockGridConfiguration configuration, string groupName) {
        var group = configuration.BlockGroups.FirstOrDefault(x => x.Name.EqualsInvariant(groupName));

        if (group == null) {
            throw new Exception($"No block group found with name {groupName.Quote()}");
        }

        return group.Key;
    }

    private Guid GetAreaKey(string elementTypeAlias, string areaAlias, BlockGridConfiguration existing) {
        var existingBlock = existing?.Blocks.FirstOrDefault(x => {
            var elementType = _contentTypeService.Get(x.ContentElementTypeKey);

            return elementType != null && elementType.Alias.EqualsInvariant(elementTypeAlias);
        });
        var existingArea = existingBlock?.Areas.FirstOrDefault(x => x.Alias.EqualsInvariant(areaAlias));

        return existingArea?.Key ?? UmbracoId.Deterministic(IdScope.BlockArea, elementTypeAlias, areaAlias);
    }

    private IContentType ResolveElementType(string alias) {
        var elementType = _contentTypeService.Get(alias);

        if (elementType == null) {
            throw new Exception($"No element type found with alias {alias.Quote()}");
        }

        return elementType;
    }
}

public class BlockGridBlockBuilder {
    private readonly List<BlockGridAreaBuilder> _areas = [];
    private readonly List<int> _columnSpans = [];

    private bool _allowAtRoot = true;
    private bool _allowInAreas = true;
    private int? _areaGridColumns;
    private string _backgroundColor;
    private string _editorSize;
    private string _groupName;
    private bool _hideContentEditorInOverlay;
    private string _iconColor;
    private bool _inlineEditing;
    private string _label;
    private int? _rowMaxSpan;
    private int? _rowMinSpan;
    private string _settingsElementTypeAlias;
    private string _stylesheet;
    private string _thumbnail;
    private string _view;

    public BlockGridBlockBuilder(string elementTypeAlias) {
        ElementTypeAlias = elementTypeAlias;
    }

    public BlockGridAreaBuilder AddArea(string alias) {
        var area = new BlockGridAreaBuilder(alias);

        _areas.Add(area);

        return area;
    }

    public BlockGridBlockBuilder AreaGridColumns(int areaGridColumns) {
        _areaGridColumns = areaGridColumns;

        return this;
    }

    public BlockGridBlockBuilder BackgroundColor(string backgroundColor) {
        _backgroundColor = backgroundColor;

        return this;
    }

    public BlockGridBlockBuilder ColumnSpans(params int[] columnSpans) {
        _columnSpans.AddRange(columnSpans);

        return this;
    }

    public BlockGridBlockBuilder DisallowAtRoot() {
        _allowAtRoot = false;

        return this;
    }

    public BlockGridBlockBuilder DisallowInAreas() {
        _allowInAreas = false;

        return this;
    }

    public BlockGridBlockBuilder EditorSize(string editorSize) {
        _editorSize = editorSize;

        return this;
    }

    public BlockGridBlockBuilder HideContentEditorInOverlay() {
        _hideContentEditorInOverlay = true;

        return this;
    }

    public BlockGridBlockBuilder IconColor(string iconColor) {
        _iconColor = iconColor;

        return this;
    }

    public BlockGridBlockBuilder InGroup(string groupName) {
        _groupName = groupName;

        return this;
    }

    public BlockGridBlockBuilder InlineEditing() {
        _inlineEditing = true;

        return this;
    }

    public BlockGridBlockBuilder Label(string label) {
        _label = label;

        return this;
    }

    public BlockGridBlockBuilder RowSpan(int? min, int? max) {
        _rowMinSpan = min;
        _rowMaxSpan = max;

        return this;
    }

    public BlockGridBlockBuilder Stylesheet(string stylesheet) {
        _stylesheet = stylesheet;

        return this;
    }

    public BlockGridBlockBuilder Thumbnail(string thumbnail) {
        _thumbnail = thumbnail;

        return this;
    }

    public BlockGridBlockBuilder View(string view) {
        _view = view;

        return this;
    }

    public BlockGridBlockBuilder WithSettings(string settingsElementTypeAlias) {
        _settingsElementTypeAlias = settingsElementTypeAlias;

        return this;
    }

    public string ElementTypeAlias { get; }

    public BlockGridConfiguration.BlockGridBlockConfiguration Build(Func<string, IContentType> resolveElementType,
                                                                    Func<string, Guid> findGroupKey,
                                                                    Func<string, Guid> getAreaKey) {
        var block = new BlockGridConfiguration.BlockGridBlockConfiguration();

        block.ContentElementTypeKey = resolveElementType(ElementTypeAlias).Key;
        block.AllowAtRoot = _allowAtRoot;
        block.AllowInAreas = _allowInAreas;
        block.Areas = _areas.Select(x => x.Build(getAreaKey, resolveElementType, findGroupKey)).ToArray();
        block.BackgroundColor = _backgroundColor;
        block.EditorSize = _editorSize;
        block.ColumnSpanOptions = _columnSpans.Select(BuildColumnSpanOption).ToArray();
        block.ForceHideContentEditorInOverlay = _hideContentEditorInOverlay;
        block.IconColor = _iconColor;
        block.InlineEditing = _inlineEditing;
        block.Label = _label;
        block.RowMaxSpan = _rowMaxSpan;
        block.RowMinSpan = _rowMinSpan;
        block.Stylesheet = _stylesheet;
        block.Thumbnail = _thumbnail;
        block.View = _view;

        if (_areaGridColumns.HasValue()) {
            block.AreaGridColumns = _areaGridColumns;
        }

        if (_groupName.HasValue()) {
            block.GroupKey = findGroupKey(_groupName).ToString();
        }

        if (_settingsElementTypeAlias.HasValue()) {
            block.SettingsElementTypeKey = resolveElementType(_settingsElementTypeAlias).Key;
        }

        return block;
    }

    private BlockGridConfiguration.BlockGridColumnSpanOption BuildColumnSpanOption(int span) {
        var option = new BlockGridConfiguration.BlockGridColumnSpanOption();

        option.ColumnSpan = span;

        return option;
    }
}

public class BlockGridAreaBuilder {
    private readonly string _alias;
    private readonly List<(string ElementTypeAlias, string GroupName, int? Min, int? Max)> _allowances = [];

    private int? _columnSpan;
    private string _createLabel;
    private int? _maxAllowed;
    private int? _minAllowed;
    private int? _rowSpan;

    public BlockGridAreaBuilder(string alias) {
        _alias = alias;
    }

    public BlockGridAreaBuilder AllowElementType(string elementTypeAlias, int? min = null, int? max = null) {
        _allowances.Add((elementTypeAlias, null, min, max));

        return this;
    }

    public BlockGridAreaBuilder AllowGroup(string groupName, int? min = null, int? max = null) {
        _allowances.Add((null, groupName, min, max));

        return this;
    }

    public BlockGridAreaBuilder ColumnSpan(int columnSpan) {
        _columnSpan = columnSpan;

        return this;
    }

    public BlockGridAreaBuilder CreateLabel(string createLabel) {
        _createLabel = createLabel;

        return this;
    }

    public BlockGridAreaBuilder Limit(int? min, int? max) {
        _minAllowed = min;
        _maxAllowed = max;

        return this;
    }

    public BlockGridAreaBuilder RowSpan(int rowSpan) {
        _rowSpan = rowSpan;

        return this;
    }

    public BlockGridConfiguration.BlockGridAreaConfiguration Build(Func<string, Guid> getAreaKey,
                                                                    Func<string, IContentType> resolveElementType,
                                                                    Func<string, Guid> findGroupKey) {
        var area = new BlockGridConfiguration.BlockGridAreaConfiguration();

        area.Key = getAreaKey(_alias);
        area.Alias = _alias;
        area.ColumnSpan = _columnSpan;
        area.CreateLabel = _createLabel;
        area.MaxAllowed = _maxAllowed;
        area.MinAllowed = _minAllowed;
        area.RowSpan = _rowSpan;
        area.SpecifiedAllowance = _allowances.Select(x => BuildAllowance(x, resolveElementType, findGroupKey))
                                             .ToArray();

        return area;
    }

    private BlockGridConfiguration.BlockGridAreaConfigurationSpecifiedAllowance BuildAllowance(
        (string ElementTypeAlias, string GroupName, int? Min, int? Max) allowance,
        Func<string, IContentType> resolveElementType,
        Func<string, Guid> findGroupKey) {
        var specifiedAllowance = new BlockGridConfiguration.BlockGridAreaConfigurationSpecifiedAllowance();

        specifiedAllowance.MinAllowed = allowance.Min;
        specifiedAllowance.MaxAllowed = allowance.Max;

        if (allowance.ElementTypeAlias.HasValue()) {
            specifiedAllowance.ElementTypeKey = resolveElementType(allowance.ElementTypeAlias).Key;
        } else {
            specifiedAllowance.GroupKey = findGroupKey(allowance.GroupName);
        }

        return specifiedAllowance;
    }
}

using N3O.Umbraco.Utilities;
using Perplex.ContentBlocks.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Blocks;

public class BlockDefinition : Value, IContentBlockDefinition {
    private readonly IReadOnlyList<Guid> _categoryIds;

    public BlockDefinition(Guid id,
                           string alias,
                           string name,
                           string description,
                           string icon,
                           string folder,
                           string previewImage,
                           IEnumerable<BlockCategory> blockCategories,
                           IEnumerable<LayoutDefinition> layoutDefinitions,
                           IEnumerable<string> limitToContentTypes,
                           IEnumerable<string> limitToCultures) {
        Id = id;
        Alias = alias;
        Name = name;
        Description = description;
        Icon = icon;
        Folder = folder;
        PreviewImage = previewImage;
        DataTypeKey = UmbracoId.Generate(IdScope.BlockDataType, alias);
        BlockCategories = blockCategories.ToList();
        LayoutDefinitions = layoutDefinitions.ToList();
        LimitToDocumentTypes = limitToContentTypes;
        LimitToCultures = limitToCultures;
    
        _categoryIds = blockCategories.Select(x => x.Id).ToList();
    }

    public Guid Id { get; }
    public string Alias { get; }
    public string Name { get; }
    public string Description { get; }
    public string Icon { get; }
    public string Folder { get; }
    public string PreviewImage { get; }
    public Guid? DataTypeKey { get; }
    public int? DataTypeId => null;
    public IReadOnlyList<BlockCategory> BlockCategories { get; }
    public IEnumerable<Guid> CategoryIds => _categoryIds;
    public IReadOnlyList<LayoutDefinition> LayoutDefinitions { get; }
    public IEnumerable<IContentBlockLayout> Layouts => LayoutDefinitions;
    public IEnumerable<string> LimitToDocumentTypes { get; }
    public IEnumerable<string> LimitToCultures { get; }
}

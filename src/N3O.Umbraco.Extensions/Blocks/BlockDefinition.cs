using N3O.Umbraco.Extensions;
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
                           string previewImage,
                           IEnumerable<BlockCategory> categories,
                           IEnumerable<LayoutDefinition> layouts,
                           IEnumerable<string> limitToContentTypes,
                           IEnumerable<string> limitToCultures) {
        Id = id;
        Alias = alias;
        Name = name;
        Description = description;
        PreviewImage = previewImage;
        DataTypeKey = Id.Increment();
        Categories = categories.OrEmpty().ToList();
        Layouts = layouts.OrEmpty().ToList();
        LimitToDocumentTypes = limitToContentTypes;
        LimitToCultures = limitToCultures;
        
        _categoryIds = Categories.Select(x => x.Id).ToList();
    }

    public Guid Id { get; }
    public string Alias { get; }
    public string Name { get; }
    public string Description { get; }
    public string PreviewImage { get; }
    public Guid? DataTypeKey { get; }
    public IReadOnlyList<BlockCategory> Categories { get; }
    public IReadOnlyList<LayoutDefinition> Layouts { get; }
    public IEnumerable<string> LimitToDocumentTypes { get; }
    public IEnumerable<string> LimitToCultures { get; }
    
    int? IContentBlockDefinition.DataTypeId => null;
    IEnumerable<Guid> IContentBlockDefinition.CategoryIds => _categoryIds;
    IEnumerable<IContentBlockLayout> IContentBlockDefinition.Layouts => Layouts;
}
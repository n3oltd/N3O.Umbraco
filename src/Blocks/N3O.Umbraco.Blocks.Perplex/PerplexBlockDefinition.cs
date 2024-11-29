using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using Perplex.ContentBlocks.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlockDefinition : Value, IContentBlockDefinition {
    private readonly IReadOnlyList<Guid> _categoryIds;

    public PerplexBlockDefinition(Guid id,
                                  string alias,
                                  string name,
                                  string description,
                                  string icon,
                                  string folder,
                                  string previewImage,
                                  IEnumerable<PerplexBlockCategory> blockCategories,
                                  IEnumerable<LayoutDefinition> layouts,
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
        Layouts = layouts.ToList();
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
    public IReadOnlyList<PerplexBlockCategory> BlockCategories { get; }
    public IEnumerable<Guid> CategoryIds => _categoryIds;
    public IReadOnlyList<LayoutDefinition> Layouts { get; }
    public IEnumerable<string> LimitToDocumentTypes { get; }
    public IEnumerable<string> LimitToCultures { get; }

    [JsonIgnore]
    IEnumerable<IContentBlockLayout> IContentBlockDefinition.Layouts => Layouts;
}

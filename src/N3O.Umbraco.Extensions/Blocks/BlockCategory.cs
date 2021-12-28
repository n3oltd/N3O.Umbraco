using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Perplex.ContentBlocks.Categories;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Blocks;

public class BlockCategory : Lookup, IContentBlockCategory {
    public BlockCategory(string id, string name, string icon)
        : base(id) {
        Id = id.GetDeterministicHashCode(true).ToGuid();
        Name = name;
        Icon = icon;
        IsHidden = false;
        IsEnabledForHeaders = false;
        IsDisabledForBlocks = false;
    }
    
    public new Guid Id { get; }
    public string Name { get; }
    public string Icon { get; }
    public bool IsHidden { get; }
    public bool IsEnabledForHeaders { get; }
    public bool IsDisabledForBlocks { get; }
}

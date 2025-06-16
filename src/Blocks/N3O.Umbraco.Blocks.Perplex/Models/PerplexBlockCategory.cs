using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using Perplex.ContentBlocks.Categories;
using System;

namespace N3O.Umbraco.Blocks.Perplex;

public class PerplexBlockCategory : Lookup, IContentBlockCategory {
    public PerplexBlockCategory(string id, string name, string icon, int order)
        : base(id) {
        Id = UmbracoId.Generate(IdScope.BlockCategory, id);
        Name = name;
        Icon = icon;
        Order = order;
        IsHidden = false;
        IsEnabledForHeaders = false;
        IsDisabledForBlocks = false;
    }

    public new Guid Id { get; }
    public string Name { get; }
    public string Icon { get; }
    public int Order { get; }
    public bool IsHidden { get; }
    public bool IsEnabledForHeaders { get; }
    public bool IsDisabledForBlocks { get; }
}

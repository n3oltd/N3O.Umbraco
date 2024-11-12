using N3O.Umbraco.Attributes;
using N3O.Umbraco.Blocks;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Blocks; 

[Order(int.MinValue)]
public class BlockCategories : StaticLookupsCollection<BlockCategory> {
    public static readonly BlockCategory Advanced = new("advanced", "Advanced", "icon-code", 2);
}
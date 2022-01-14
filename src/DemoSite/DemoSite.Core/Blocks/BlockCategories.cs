using N3O.Umbraco.Blocks;
using N3O.Umbraco.Lookups;

namespace DemoSite.Core.Blocks {
    public class BlockCategories : StaticLookupsCollection<BlockCategory> {
        public static readonly BlockCategory General = new("general", "General", "icon-brick", 1);
    }
}
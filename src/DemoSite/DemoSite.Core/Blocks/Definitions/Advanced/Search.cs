using N3O.Umbraco.Blocks;
using DemoSite.Core.Content;

namespace DemoSite.Core.Blocks.Definitions;

public class Search : BlockBuilder {
    public Search() {
        WithAlias("search");
        WithName("Search");
        WithIcon("icon-search");
        WithDescription("Displays search results, a search form and pagination links");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<SearchPage>();
        SingleLayout();
    }
}

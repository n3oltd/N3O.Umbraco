using N3O.Umbraco.Blocks;
using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.Crowdfunding.Blocks.Definitions; 

public class Crowdfunding : BlockBuilder {
    public Crowdfunding() {
        WithAlias("crowdfunding");
        WithName("Crowdfunding");
        WithIcon("icon-users-alt");
        WithDescription("Block required to enable crowdfunding functionality");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<HomePageContent>();
        SingleLayout();
    }
}
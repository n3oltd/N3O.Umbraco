using N3O.Umbraco.Blocks.Perplex;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Crowdfunding.Blocks.Definitions; 

public class CrowdfundingBlockBuilder : PerplexBlockBuilder {
    public CrowdfundingBlockBuilder() {
        var advancedCategory = StaticLookups.FindById<PerplexBlockCategory>("advanced");

        if (advancedCategory == null) {
            throw new Exception("A block category with ID advanced is required for crowdfunding");
        }
        
        WithAlias("crowdfunding");
        WithName("Crowdfunding");
        WithIcon("icon-users-alt");
        WithDescription("Block required to enable crowdfunding functionality");
        AddToCategory(advancedCategory);
        LimitTo<HomePageContent>();
        SingleLayout();
    }
}
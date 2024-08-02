using N3O.Umbraco.Blocks;

namespace N3O.Umbraco.Crowdfunding.Blocks.Definitions; 

public class Crowdfunding : BlockBuilder {
    public Crowdfunding() {
        WithAlias("crowdfunding");
        WithName("Crowdfunding");
        WithIcon("icon-tab");
        WithDescription("Crowdfunding block to render difference related pages");
        AddToCategory(BlockCategories.General);
        SingleLayout();
    }
}
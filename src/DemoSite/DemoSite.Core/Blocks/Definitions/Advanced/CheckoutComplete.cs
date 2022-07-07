using DemoSite.Content;
using N3O.Umbraco.Blocks;

namespace DemoSite.Blocks.Definitions;

public class CheckoutComplete : BlockBuilder {
    public CheckoutComplete() {
        WithAlias("checkoutComplete");
        WithName("Checkout Complete");
        WithIcon("icon-trophy");
        WithDescription("Renders the checkout complete stage");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutCompletePage>();
        SingleLayout();
    }
}

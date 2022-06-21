using DemoSite.Core.Content;
using N3O.Umbraco.Blocks;

namespace DemoSite.Core.Blocks.Definitions;

public class Checkout : BlockBuilder {
    public Checkout() {
        WithAlias("checkout");
        WithName("Checkout");
        WithIcon("icon-cash-register");
        WithDescription("Renders the checkout page");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutPage>();
        SingleLayout();
    }
}

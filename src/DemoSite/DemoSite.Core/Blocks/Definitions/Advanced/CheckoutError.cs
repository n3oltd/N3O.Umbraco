using DemoSite.Content;
using N3O.Umbraco.Blocks;

namespace DemoSite.Blocks.Definitions;

public class CheckoutError : BlockBuilder {
    public CheckoutError() {
        WithAlias("checkoutError");
        WithName("Checkout Error");
        WithIcon("icon-alert-alt");
        WithDescription("Renders the checkout error message");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutErrorPage>();
        SingleLayout();
    }
}

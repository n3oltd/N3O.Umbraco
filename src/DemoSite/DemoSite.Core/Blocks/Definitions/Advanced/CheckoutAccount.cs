using N3O.Umbraco.Blocks;
using DemoSite.Content;

namespace DemoSite.Blocks.Definitions;

public class CheckoutAccount : BlockBuilder {
    public CheckoutAccount() {
        WithAlias("checkoutAccount");
        WithName("Checkout Account");
        WithIcon("icon-user");
        WithDescription("Renders the account checkout stage");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutAccountPage>();
        SingleLayout();
    }
}

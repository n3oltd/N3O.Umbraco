using N3O.Umbraco.Blocks;
using DemoSite.Core.Content;

namespace DemoSite.Core.Blocks.Definitions {
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
}
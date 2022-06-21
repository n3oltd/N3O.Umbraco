using N3O.Umbraco.Blocks;
using DemoSite.Core.Content;

namespace DemoSite.Core.Blocks.Definitions;

public class CheckoutDonation : BlockBuilder {
    public CheckoutDonation() {
        WithAlias("checkoutDonation");
        WithName("Checkout Donation");
        WithIcon("icon-coin-dollar");
        WithDescription("Renders the donation checkout stage");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutDonationPage>();
        SingleLayout();
    }
}

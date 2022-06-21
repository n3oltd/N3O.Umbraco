using N3O.Umbraco.Blocks;
using DemoSite.Core.Content;

namespace DemoSite.Core.Blocks.Definitions;

public class CheckoutRegularGiving : BlockBuilder {
    public CheckoutRegularGiving() {
        WithAlias("checkoutRegularGiving");
        WithName("Checkout Regular Giving");
        WithIcon("icon-calendar");
        WithDescription("Renders the regular giving checkout stage");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CheckoutRegularGivingPage>();
        SingleLayout();
    }
}

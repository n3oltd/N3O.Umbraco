using N3O.Umbraco.Blocks;
using DemoSite.Content;

namespace DemoSite.Blocks.Definitions;

public class Cart : BlockBuilder {
    public Cart() {
        WithAlias("cart");
        WithName("Cart");
        WithIcon("icon-shopping-basket");
        WithDescription("Displays the cart contents");
        AddToCategory(BlockCategories.Advanced);
        LimitTo<CartPage>();
        SingleLayout();
    }
}

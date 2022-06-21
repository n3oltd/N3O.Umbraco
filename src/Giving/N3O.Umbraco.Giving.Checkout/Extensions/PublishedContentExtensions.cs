using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Extensions;

public static class PublishedContentExtensions {
    private static readonly string[] CheckoutPageAliases = {
        AliasHelper<CheckoutAccountPageContent>.ContentTypeAlias(),
        AliasHelper<CheckoutDonationPageContent>.ContentTypeAlias(),
        AliasHelper<CheckoutRegularGivingPageContent>.ContentTypeAlias(),
        AliasHelper<CheckoutCompletePageContent>.ContentTypeAlias()
    };
    
    public static bool IsCheckoutPage(this IPublishedContent page) {
        return CheckoutPageAliases.Contains(page?.ContentType.Alias, true);
    }
}

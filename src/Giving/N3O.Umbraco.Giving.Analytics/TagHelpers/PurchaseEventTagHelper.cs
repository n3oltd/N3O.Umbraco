using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Giving.Analytics.Extensions;
using N3O.Umbraco.Giving.Checkout;
using N3O.Umbraco.Json;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Analytics.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}purchase-event")]
public class PurchaseEventTagHelper : EventTagHelper {
    private readonly ICheckoutAccessor _checkoutAccessor;

    public PurchaseEventTagHelper(IJsonProvider jsonProvider, ICheckoutAccessor checkoutAccessor)
        : base(jsonProvider) {
        _checkoutAccessor = checkoutAccessor;
    }

    protected override async Task<object> GetParametersAsync() {
        var checkout = await _checkoutAccessor.GetAsync();

        return checkout.ToPurchase();
    }

    protected override string EventName => "purchase";
}

using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class PriceHandleElement : UmbracoElement<PriceHandleElement> {
    public decimal Amount => GetValue(x => x.Amount);
    public string Description => GetValue(x => x.Description);
}

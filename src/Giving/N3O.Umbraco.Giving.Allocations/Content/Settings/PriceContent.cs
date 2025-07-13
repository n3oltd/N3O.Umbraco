using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class PriceContent : UmbracoContent<PriceContent>, IPrice {
    [UmbracoProperty(AllocationsConstants.Aliases.Price.Properties.Amount)]
    public decimal Amount => GetValue(x => x.Amount);

    [UmbracoProperty(AllocationsConstants.Aliases.Price.Properties.Locked)]
    public bool Locked => GetValue(x => x.Locked);
}

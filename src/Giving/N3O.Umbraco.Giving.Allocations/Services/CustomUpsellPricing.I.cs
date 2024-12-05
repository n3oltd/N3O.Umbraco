using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations;

public interface ICustomUpsellPricing {
    Money GetPrice(IForexConverter forexConverter,
                   UpsellOfferContent upsellOfferContent,
                   Currency currency,
                   GivingType givingType,
                   Money cartTotal);
}
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.Giving; 

public interface ICustomUpsellPricing {
    Money GetPrice(IForexConverter forexConverter,
                   UpsellOfferContent upsellOfferContent,
                   Currency currency,
                   GivingType givingType,
                   Money cartTotal);
}
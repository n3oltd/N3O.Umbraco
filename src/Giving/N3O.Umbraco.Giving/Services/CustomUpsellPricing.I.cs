using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving; 

public interface ICustomUpsellPricing {
    Money GetPrice(IForexConverter forexConverter,
                   UpsellContent upsellContent,
                   Currency currency,
                   GivingType givingType,
                   Money cartTotal);
}
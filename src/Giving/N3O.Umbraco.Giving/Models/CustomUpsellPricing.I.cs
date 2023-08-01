using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Giving.Models; 

public interface ICustomUpsellPricing {
    Money GetAmount(Money regularTotal, Money donationTotal);
}
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Pricing;
using N3O.Umbraco.Giving.Pricing.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Extensions {
    public static class SponsorshipSchemeExtensions {
        public static IEnumerable<DonationType> GetDonationTypes(this SponsorshipScheme scheme) {
            if (scheme.AllowSingleDonations) {
                yield return DonationTypes.Single;
            }
        
            if (scheme.AllowRegularDonations) {
                yield return DonationTypes.Regular;
            }
        }
    
        public static Money GetPrice(this SponsorshipScheme scheme,
                                     IPricing pricing,
                                     Currency currency,
                                     DonationType donationType) {
            var price = pricing.InCurrency(scheme, currency);

            if (donationType == DonationTypes.Single) {
                price = new Money(price.Amount * 12, price.Currency);
            }

            return price;
        }
    }
}

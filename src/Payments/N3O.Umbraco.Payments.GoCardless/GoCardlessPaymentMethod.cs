using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.GoCardless.Content;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.GoCardless {
    public class GoCardlessPaymentMethod : PaymentMethod {
        public GoCardlessPaymentMethod()
            : base("goCardless", "GoCardless", false, null, typeof(GoCardlessCredential)) { }

        public override bool IsAvailable(IContentCache contentCache, Country country, Currency currency) {
            var settings = contentCache.Single<GoCardlessSettingsContent>();

            if (settings == null) {
                return false;
            }

            if (!country.Iso3Code.EqualsInvariant(GoCardlessConstants.Codes.Countries.UnitedKingdom)) {
                return false;
            }

            if (!currency.Name.EqualsInvariant(GoCardlessConstants.Codes.Currencies.GBP)) {
                return false;
            }

            return true;
        }
    }
}
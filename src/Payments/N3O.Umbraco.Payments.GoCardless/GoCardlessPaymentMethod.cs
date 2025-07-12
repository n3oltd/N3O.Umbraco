using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.GoCardless.Content;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.GoCardless;

public class GoCardlessPaymentMethod : PaymentMethod {
    public GoCardlessPaymentMethod() : base("goCardless", "GoCardless", null, typeof(GoCardlessCredential)) { }

    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<GoCardlessSettingsContent>.ContentTypeAlias();
    }
    
    public override bool IsAvailable(Country country, Currency currency) {
        if (!country.Iso3Code.EqualsInvariant(GoCardlessConstants.Codes.Iso3CountryCodes.UnitedKingdom)) {
            return false;
        }

        if (!currency.Code.EqualsInvariant(GoCardlessConstants.Codes.Currencies.GBP)) {
            return false;
        }

        return true;
    }
}

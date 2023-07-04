using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.DirectDebitUK.Content;
using N3O.Umbraco.Payments.DirectDebitUK.Models;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.DirectDebitUK;

public class DirectDebitUKPaymentMethod : PaymentMethod {
    public DirectDebitUKPaymentMethod()
        : base("directDebitUK", "Direct Debit UK", null, typeof(DirectDebitUKCredential)) { }

    public override string GetSettingsContentTypeAlias() {
        return AliasHelper<DirectDebitUKSettingsContent>.ContentTypeAlias();
    }
    
    public override bool IsAvailable(Country country, Currency currency) {
        if (!country.Iso3Code.EqualsInvariant(DirectDebitUKConstants.Codes.Iso3CountryCodes.UnitedKingdom)) {
            return false;
        }

        if (!currency.Name.EqualsInvariant(DirectDebitUKConstants.Codes.Currencies.GBP)) {
            return false;
        }

        return true;
    }
}
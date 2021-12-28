using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.TaxRelief.UnitedKingdom {
    public class UnitedKingdomTaxReliefScheme : ITaxReliefScheme {
        public Money GetAllowanceValue(LocalDate date, Money value) {
            return new Money(value.Amount * 0.25m, value.Currency);
        }

        public bool IsEligible(Country residenceCountry, bool isOrganization) {
            return !isOrganization &&
                   residenceCountry.Iso3Code == UnitedKingdomConstants.Iso3Codes.UnitedKingdom;
        }
    }
}

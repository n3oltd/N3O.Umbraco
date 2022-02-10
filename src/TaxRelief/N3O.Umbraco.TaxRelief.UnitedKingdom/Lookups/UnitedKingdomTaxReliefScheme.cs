using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.TaxRelief.Lookups;
using NodaTime;

namespace N3O.Umbraco.TaxRelief.UnitedKingdom.Lookups {
    public class UnitedKingdomTaxReliefScheme : TaxReliefScheme {
        public UnitedKingdomTaxReliefScheme() : base("unitedKingdom", "Gift Aid") { }

        public override Money GetAllowanceValue(LocalDate date, Money value) {
            return new Money(value.Amount * 0.25m, value.Currency);
        }

        public override bool IsEligible(Country residenceCountry, bool isOrganization) {
            return !isOrganization &&
                   residenceCountry.Iso3Code == UnitedKingdomConstants.Iso3Codes.UnitedKingdom;
        }
    }
}

using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.TaxRelief;

public interface ITaxReliefScheme {
    Money GetAllowanceValue(LocalDate date, Money value);
    bool IsEligible(Country residenceCountry, bool isOrganization);
}

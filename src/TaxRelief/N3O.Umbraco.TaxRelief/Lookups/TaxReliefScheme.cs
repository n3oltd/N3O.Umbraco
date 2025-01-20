using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using NodaTime;

namespace N3O.Umbraco.TaxRelief.Lookups;

public abstract class TaxReliefScheme : NamedLookup {
    protected TaxReliefScheme(string id, string name, ProcessorType type) : base(id, name) {
        Type = type;
    }

    public ProcessorType Type { get; set; }

    public abstract Money GetAllowanceValue(LocalDate date, Money value);
    public abstract bool IsEligible(Country residenceCountry, bool isOrganization);
}

public class TaxReliefSchemes : TypesLookupsCollection<TaxReliefScheme> { }

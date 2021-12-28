using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class DonationType : NamedLookup {
    public DonationType(string id, string name) : base(id, name) { }
}

public class DonationTypes : StaticLookupsCollection<DonationType> {
    public static readonly DonationType Regular = new("regular", "Regular");
    public static readonly DonationType Single = new("single", "Single");
}

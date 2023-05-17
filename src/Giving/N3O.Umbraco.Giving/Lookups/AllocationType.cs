using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class AllocationType : NamedLookup {
    public AllocationType(string id, string name) : base(id, name) { }
}

public class AllocationTypes : StaticLookupsCollection<AllocationType> {
    public static readonly AllocationType Feedback = new("feedback", "Feedback");
    public static readonly AllocationType Fund = new("fund", "Fund");
    public static readonly AllocationType Sponsorship = new("sponsorship", "Sponsorship");
}

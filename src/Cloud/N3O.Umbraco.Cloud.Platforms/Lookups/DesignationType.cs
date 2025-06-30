using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DesignationType : NamedLookup {
    public DesignationType(string id, string name) : base(id, name) { }
}

public class DesignationTypes : StaticLookupsCollection<DesignationType> {
    public static readonly DesignationType Fund = new("fund", "Fund");
    public static readonly DesignationType Feedback = new("feedback", "Feedback");
    public static readonly DesignationType Sponsorship = new("sponsorship", "Sponsorship");
}
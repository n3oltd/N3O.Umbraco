using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.CrowdFunding.Lookups;

public class CrowdfunderType : NamedLookup {
    public CrowdfunderType(string id, string name) : base(id, name) { }
}

public class CrowdfunderTypes : StaticLookupsCollection<CrowdfunderType> {
    public static readonly CrowdfunderType Campaign = new("campaign", "Campaign");
    public static readonly CrowdfunderType Fundraiser = new("fundraiser", "Fundraiser");
}
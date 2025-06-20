using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Engage.Lookups;

public class CrowdfunderType : KeyedNamedLookup {
    public CrowdfunderType(string id, string name, uint key) : base(id, name, key) { }
}

public class CrowdfunderTypes : StaticLookupsCollection<CrowdfunderType> {
    public static readonly CrowdfunderType Campaign = new("campaign", "Campaign", 1);
    public static readonly CrowdfunderType Fundraiser = new("fundraiser", "Fundraiser", 2);
}
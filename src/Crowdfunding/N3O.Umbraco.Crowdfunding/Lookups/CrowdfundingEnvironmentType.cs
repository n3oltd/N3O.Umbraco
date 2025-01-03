using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CrowdfundingEnvironmentType : NamedLookup {
    public CrowdfundingEnvironmentType(string id, string name) : base(id, name) { }
}

public class CrowdfundingEnvironmentTypes : StaticLookupsCollection<CrowdfundingEnvironmentType> {
    public static readonly CrowdfundingEnvironmentType Staging = new("staging", "Staging");
    public static readonly CrowdfundingEnvironmentType Production = new("production", "Production");
}
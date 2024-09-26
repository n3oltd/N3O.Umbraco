using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class CrowdfunderStatus : NamedLookup {
    public CrowdfunderStatus(string id, string name) : base(id, name) { }
}

public class CrowdfunderStatuses : StaticLookupsCollection<CrowdfunderStatus> {
    public static readonly CrowdfunderStatus Active = new("active", "Active");
    public static readonly CrowdfunderStatus Draft = new("draft", "Draft");
    public static readonly CrowdfunderStatus Ended = new("ended", "Ended");
    public static readonly CrowdfunderStatus Inactive = new("inactive", "Inactive");
}
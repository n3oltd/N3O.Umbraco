using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Crowdfunding.Lookups;

public class CrowdfundingPageStatus : NamedLookup {
    public CrowdfundingPageStatus(string id, string name) : base(id, name) { }
}

public class CrowdfundingPageStatuses : StaticLookupsCollection<CrowdfundingPageStatus> {
    public static CrowdfundingPageStatus Closed = new("closed", "Closed");
    public static CrowdfundingPageStatus Open = new("open", "Open");
    public static CrowdfundingPageStatus Pending = new("pending", "Pending");
}
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class FundraiserStatus : NamedLookup {
    public FundraiserStatus(string id, string name) : base(id, name) { }
}

public class FundraiserStatuses : StaticLookupsCollection<FundraiserStatus> {
    public static FundraiserStatus Closed = new("closed", "Closed");
    public static FundraiserStatus Open = new("open", "Open");
    public static FundraiserStatus Pending = new("pending", "Pending");
}
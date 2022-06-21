using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models;

public class FundAllocationRes : IFundAllocation {
    public DonationItem DonationItem { get; set; }
}

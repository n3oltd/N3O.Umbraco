using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundAllocationRes : IFundAllocation {
    public DonationItem DonationItem { get; set; }
}

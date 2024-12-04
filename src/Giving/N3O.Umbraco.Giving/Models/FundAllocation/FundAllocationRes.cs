using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Models;

public class FundAllocationRes : IFundAllocation {
    public DonationItem DonationItem { get; set; }
}

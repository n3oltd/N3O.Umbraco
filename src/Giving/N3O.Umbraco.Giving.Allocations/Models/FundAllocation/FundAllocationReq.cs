using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models; 

public class FundAllocationReq : IFundAllocation {
    [Name("Donation Item")]
    public DonationItem DonationItem { get; set; }
}
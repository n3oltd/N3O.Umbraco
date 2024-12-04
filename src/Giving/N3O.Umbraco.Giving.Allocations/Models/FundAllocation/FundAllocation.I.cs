using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFundAllocation {
    DonationItem DonationItem { get; }
}

using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Engage.Models;

public interface IFundCrowdfunderGoal {
    DonationItem GetDonationItem(ILookups lookups);
}
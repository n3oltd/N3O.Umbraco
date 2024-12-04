using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Checkout.Entities;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserContributionReceivedViewModel : FundraiserNotificationViewModel {
    public FundraiserContributionReceivedViewModel(FundraiserContent fundraiser,
                                                   Checkout checkout,
                                                   Allocation allocation,
                                                   CrowdfunderData contribution)
        : base(fundraiser) {
        Checkout = checkout;
        Allocation = allocation;
        Contribution = contribution;
    }

    public Checkout Checkout { get; }
    public Allocation Allocation { get; }
    public CrowdfunderData Contribution { get; }
}
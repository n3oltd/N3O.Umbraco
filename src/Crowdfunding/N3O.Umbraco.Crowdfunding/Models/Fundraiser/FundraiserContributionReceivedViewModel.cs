using N3O.Umbraco.Giving.Checkout.Entities;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserContributionReceivedViewModel {
    public FundraiserContributionReceivedViewModel(FundraiserNotificationViewModel fundraiser,
                                                   Checkout checkout,
                                                   Allocation allocation,
                                                   CrowdfunderData contribution) {
        Fundraiser = fundraiser;
        Checkout = checkout;
        Allocation = allocation;
        Contribution = contribution;
    }

    public Checkout Checkout { get; }
    public Allocation Allocation { get; }
    public CrowdfunderData Contribution { get; }
    public FundraiserNotificationViewModel Fundraiser { get; }
}
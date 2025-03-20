namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserContributionReceivedViewModel {
    public FundraiserContributionReceivedViewModel(FundraiserContentViewModel fundraiser,
                                                   object checkout,
                                                   object allocation,
                                                   CrowdfunderData contribution) {
        Fundraiser = fundraiser;
        Checkout = checkout;
        Allocation = allocation;
        Contribution = contribution;
    }

    public object Checkout { get; }
    public object Allocation { get; }
    public CrowdfunderData Contribution { get; }
    public FundraiserContentViewModel Fundraiser { get; }
}
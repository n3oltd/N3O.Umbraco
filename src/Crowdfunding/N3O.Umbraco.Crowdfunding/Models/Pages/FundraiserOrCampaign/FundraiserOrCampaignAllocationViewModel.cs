using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignAllocationViewModel {
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public FundDimension1Value FundDimension1Value { get; set; }
    public FundDimension2Value FundDimension2Value { get; set; }
    public FundDimension3Value FundDimension3Value { get; set; }
    public FundDimension4Value FundDimension4Value { get; set; }
    public AllocationType Type { get; set; }
    public DonationItem DonationItem { get; set; }
    public FeedbackScheme FeedbackScheme { get; set; }
    public IReadOnlyList<FundraiserOrCampaignPriceHandleViewModel> PriceHandles { get; set; }

    public static FundraiserOrCampaignAllocationViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                              GoalElement goal) {
        var allocation = new FundraiserOrCampaignAllocationViewModel();

        allocation.Title = goal.Title;
        allocation.Amount = goal.Amount;
        allocation.FundDimension1Value = goal.FundDimension1;
        allocation.FundDimension2Value = goal.FundDimension2;
        allocation.FundDimension3Value = goal.FundDimension3;
        allocation.FundDimension4Value = goal.FundDimension4;
        allocation.Type = goal.Type;
        allocation.DonationItem = goal.Fund?.DonationItem;
        allocation.FeedbackScheme = goal.Feedback?.Scheme;
        allocation.PriceHandles = goal.PriceHandles.ToReadOnlyList(x => FundraiserOrCampaignPriceHandleViewModel.For(crowdfundingHelper, x));

        return allocation;
    }
}
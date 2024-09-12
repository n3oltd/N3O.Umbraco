using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignGoalViewModel {
    public string Title { get; private set; }
    public decimal Amount { get; private set; }
    public FundDimension1Value FundDimension1Value { get; private set; }
    public FundDimension2Value FundDimension2Value { get; private set; }
    public FundDimension3Value FundDimension3Value { get; private set; }
    public FundDimension4Value FundDimension4Value { get; private set; }
    public AllocationType Type { get; private set; }
    public DonationItem DonationItem { get; private set; }
    public FeedbackScheme FeedbackScheme { get; private set; }
    public IReadOnlyList<FundraiserOrCampaignPriceHandleViewModel> PriceHandles { get; private set; }

    public static FundraiserOrCampaignGoalViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                        GoalElement goal) {
        var viewModel = new FundraiserOrCampaignGoalViewModel();

        viewModel.Title = goal.Title;
        viewModel.Amount = goal.Amount;
        viewModel.FundDimension1Value = goal.FundDimension1;
        viewModel.FundDimension2Value = goal.FundDimension2;
        viewModel.FundDimension3Value = goal.FundDimension3;
        viewModel.FundDimension4Value = goal.FundDimension4;
        viewModel.Type = goal.Type;
        viewModel.DonationItem = goal.Fund?.DonationItem;
        viewModel.FeedbackScheme = goal.Feedback?.Scheme;
        viewModel.PriceHandles = goal.PriceHandles.ToReadOnlyList(x => FundraiserOrCampaignPriceHandleViewModel.For(crowdfundingHelper, x));

        return viewModel;
    }
}
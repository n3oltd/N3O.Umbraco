using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderGoalViewModel {
    public string Id { get; private set; }
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public FundDimension1Value FundDimension1Value { get; private set; }
    public FundDimension2Value FundDimension2Value { get; private set; }
    public FundDimension3Value FundDimension3Value { get; private set; }
    public FundDimension4Value FundDimension4Value { get; private set; }
    public AllocationType Type { get; private set; }
    public DonationItem DonationItem { get; private set; }
    public FeedbackScheme FeedbackScheme { get; private set; }
    public IReadOnlyList<CrowdfunderPriceHandleViewModel> PriceHandles { get; private set; }

    public static async Task<CrowdfunderGoalViewModel> ForAsync(Currency crowdfunderCurrency,
                                                                IForexConverter forexConverter,
                                                                ILookups lookups,
                                                                GoalElement goal) {
        var fundDimensions = goal.GetFundDimensionValues(lookups);
        
        var viewModel = new CrowdfunderGoalViewModel();

        viewModel.Id = goal.Id;
        viewModel.Name = goal.Name;
        viewModel.Amount = goal.Amount;
        viewModel.FundDimension1Value = fundDimensions.Dimension1;
        viewModel.FundDimension2Value = fundDimensions.Dimension2;
        viewModel.FundDimension3Value = fundDimensions.Dimension3;
        viewModel.FundDimension4Value = fundDimensions.Dimension4;
        viewModel.Type = goal.Type;
        viewModel.DonationItem = goal.Fund?.GetDonationItem(lookups);
        viewModel.FeedbackScheme = goal.Feedback?.GetScheme(lookups);
        viewModel.PriceHandles = await goal.PriceHandles
                                           .ToReadOnlyListAsync(async x => await CrowdfunderPriceHandleViewModel.ForAsync(crowdfunderCurrency,
                                                                                                                          forexConverter,
                                                                                                                          lookups,
                                                                                                                          x));

        return viewModel;
    }
}
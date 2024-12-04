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
        var viewModel = new CrowdfunderGoalViewModel();

        viewModel.Id = goal.Id;
        viewModel.Name = goal.Name;
        viewModel.Amount = goal.Amount;
        viewModel.FundDimension1Value = goal.FundDimension1;
        viewModel.FundDimension2Value = goal.FundDimension2;
        viewModel.FundDimension3Value = goal.FundDimension3;
        viewModel.FundDimension4Value = goal.FundDimension4;
        viewModel.Type = goal.Type;
        viewModel.DonationItem = goal.Fund?.DonationItem;
        viewModel.FeedbackScheme = goal.Feedback?.Scheme;
        viewModel.PriceHandles = await goal.PriceHandles
                                           .ToReadOnlyListAsync(async x => await CrowdfunderPriceHandleViewModel.ForAsync(crowdfunderCurrency,
                                                                                                                          forexConverter,
                                                                                                                          lookups,
                                                                                                                          x));

        return viewModel;
    }
}
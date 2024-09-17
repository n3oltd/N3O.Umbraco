using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderGoalViewModel {
    public Guid Id { get; private set; }
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

    public static CrowdfunderGoalViewModel For(Currency crowdfunderCurrency, GoalElement goal) {
        var viewModel = new CrowdfunderGoalViewModel();

        viewModel.Id = goal.GoalId;
        viewModel.Name = goal.Name;
        viewModel.Amount = goal.Amount;
        viewModel.FundDimension1Value = goal.FundDimension1;
        viewModel.FundDimension2Value = goal.FundDimension2;
        viewModel.FundDimension3Value = goal.FundDimension3;
        viewModel.FundDimension4Value = goal.FundDimension4;
        viewModel.Type = goal.Type;
        viewModel.DonationItem = goal.Fund?.DonationItem;
        viewModel.FeedbackScheme = goal.Feedback?.Scheme;
        viewModel.PriceHandles = goal.PriceHandles
                                     .ToReadOnlyList(x => CrowdfunderPriceHandleViewModel.For(crowdfunderCurrency, x));

        return viewModel;
    }
}
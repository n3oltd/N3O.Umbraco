using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderProgressViewModel {
    public int SupportersCount { get; private set; }
    public Money RaisedAmount { get; private set; }
    public Money TargetAmount { get; private set; }
    public decimal PercentageCompleted => RaisedAmount.Amount / TargetAmount.Amount * 100m;
        
    public static CrowdfunderProgressViewModel For(Currency crowdfunderCurrency,
                                                   IEnumerable<Contribution> contributions,
                                                   IEnumerable<GoalElement> goals) {
        var viewModel = new CrowdfunderProgressViewModel();
            
        viewModel.TargetAmount = new Money(goals.Sum(x => x.Amount), crowdfunderCurrency);
        viewModel.RaisedAmount = new Money(contributions.Sum(x => x.CrowdfunderAmount), crowdfunderCurrency);
        viewModel.SupportersCount = contributions.Count();

        return viewModel;
    }
}
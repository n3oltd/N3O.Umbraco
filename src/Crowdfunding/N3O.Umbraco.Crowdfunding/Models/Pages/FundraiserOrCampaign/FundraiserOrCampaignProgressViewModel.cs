using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignProgressViewModel {
    public int SupportersCount { get; private set; }
    public Money RaisedAmount { get; private set; }
    public Money TargetAmount { get; private set; }
    public decimal PercentageCompleted => RaisedAmount.Amount / TargetAmount.Amount * 100m;
        
    public static FundraiserOrCampaignProgressViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                            IEnumerable<OnlineContribution> onlineContributions,
                                                            IEnumerable<GoalElement> goals) {
        var viewModel = new FundraiserOrCampaignProgressViewModel();
            
        viewModel.TargetAmount = crowdfundingHelper.GetQuoteMoney(goals.Sum(x => x.Amount));
        viewModel.RaisedAmount = crowdfundingHelper.GetQuoteMoney(onlineContributions.Sum(x => x.BaseAmount));
        viewModel.SupportersCount = onlineContributions.Count();

        return viewModel;
    }
}
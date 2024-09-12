using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignProgressViewModel {
    public int SupportersCount { get; set; }
    public Money RaisedAmount { get; set; }
    public Money TargetAmount { get; set; }
    public decimal PercentageCompleted { get; set; }
        
    public static FundraiserOrCampaignProgressViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                            IEnumerable<OnlineContribution> onlineContributions,
                                                            IEnumerable<GoalElement> goals) {
        var progress = new FundraiserOrCampaignProgressViewModel();
            
        progress.TargetAmount = crowdfundingHelper.GetQuoteMoney(goals.Sum(x => x.Amount));
        progress.RaisedAmount = crowdfundingHelper.GetQuoteMoney(onlineContributions.Sum(x => x.BaseAmount));
        progress.SupportersCount = onlineContributions.Count();
        progress.PercentageCompleted = progress.RaisedAmount.Amount / progress.TargetAmount.Amount * 100m;

        return progress;
    }
}
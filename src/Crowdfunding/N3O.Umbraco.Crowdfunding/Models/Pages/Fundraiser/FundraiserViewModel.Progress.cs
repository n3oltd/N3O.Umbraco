using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Financial;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public partial class FundraiserViewModel {
    public class ProgressInfo {
        public int SupportersCount { get; set; }
        public Money RaisedAmount { get; set; }
        public Money TargetAmount { get; set; }
        public decimal PercentageCompleted { get; set; }
        
        public static ProgressInfo For(ICrowdfundingHelper crowdfundingHelper,
                                       IEnumerable<CrowdfundingContribution> contributions,
                                       FundraiserContent fundraiser) {
            var progress = new ProgressInfo();
            
            progress.TargetAmount = crowdfundingHelper.GetQuoteMoney(fundraiser.Allocations.Sum(x => x.Amount));
            progress.RaisedAmount = crowdfundingHelper.GetQuoteMoney(contributions.Sum(x => x.BaseAmount));
            progress.SupportersCount = contributions.Count();
            progress.PercentageCompleted = progress.RaisedAmount.Amount / progress.TargetAmount.Amount * 100m;

            return progress;
        }
    }
}
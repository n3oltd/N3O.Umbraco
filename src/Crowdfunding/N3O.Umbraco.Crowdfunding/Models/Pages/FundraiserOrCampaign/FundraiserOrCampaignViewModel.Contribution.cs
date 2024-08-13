using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Crowdfunding.UIBuilder;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using NodaTime;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class FundraiserOrCampaignViewModel<TContent> {
    public class Contribution {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string AvatarLink { get; set; }
        public LocalDate DonatedAt { get; set; }
        public bool IsAnonymous { get; set; }
        public Money Amount { get; set; }

        public static Contribution For(ICrowdfundingHelper crowdfundingHelper,
                                       CrowdfundingContribution crowdfundingContribution) {
            var contribution = new Contribution();
            
            contribution.IsAnonymous = crowdfundingContribution.Anonymous;
            contribution.Comment = crowdfundingContribution.Comment;
            contribution.DonatedAt = crowdfundingContribution.Timestamp.ToLocalDate();
            contribution.Amount = crowdfundingHelper.GetQuoteMoney(crowdfundingContribution.BaseAmount);
            contribution.AvatarLink = (contribution.Name ?? "Anonymous").GetGravatarUrl();

            if (!contribution.IsAnonymous) {
                contribution.Name = crowdfundingContribution.Name;
            }

            return contribution;
        }
    }
}
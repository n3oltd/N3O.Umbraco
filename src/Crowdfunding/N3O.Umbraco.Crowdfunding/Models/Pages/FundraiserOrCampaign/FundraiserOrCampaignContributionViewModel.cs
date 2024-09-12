using Humanizer;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignContributionViewModel {
    public string Name { get; set; }
    public string Comment { get; set; }
    public string AvatarLink { get; set; }
    public string DonatedAt { get; set; }
    public bool IsAnonymous { get; set; }
    public Money Amount { get; set; }

    public static FundraiserOrCampaignContributionViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                                OnlineContribution src) {
        var dest = new FundraiserOrCampaignContributionViewModel();

        dest.IsAnonymous = src.Anonymous;
        dest.Comment = src.Comment;
        dest.DonatedAt = src.Timestamp.Humanize();
        dest.Amount = crowdfundingHelper.GetQuoteMoney(src.BaseAmount);
        dest.AvatarLink = (src.Name ?? "Anonymous").GetGravatarUrl();

        if (!dest.IsAnonymous) {
            dest.Name = src.Name;
        }

        return dest;
    }
}
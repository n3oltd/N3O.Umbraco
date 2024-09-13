using Humanizer;
using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.CrowdFunding.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignContributionViewModel {
    public string Name { get; private set; }
    public string Comment { get; private set; }
    public string AvatarLink { get; private set; }
    public string DonatedAt { get; private set; }
    public bool IsAnonymous { get; private set; }
    public Money Amount { get; private set; }

    public static FundraiserOrCampaignContributionViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                                IFormatter formatter,
                                                                OnlineContribution src) {
        var viewModel = new FundraiserOrCampaignContributionViewModel();

        viewModel.IsAnonymous = src.Anonymous;
        viewModel.Comment = src.Comment;
        viewModel.DonatedAt = src.Timestamp.Humanize();
        viewModel.Amount = crowdfundingHelper.GetQuoteMoney(src.BaseAmount);
        viewModel.AvatarLink = (src.Name ?? formatter.Text.Format<Strings>(s => s.Anonymous)).GetGravatarUrl();
        viewModel.Name = src.Name;

        return viewModel;
    }

    public class Strings : CodeStrings {
        public string Anonymous => "Anonymous";
    }
}
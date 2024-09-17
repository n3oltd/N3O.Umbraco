using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewCampaignPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;

    public ViewCampaignPage(IContentLocator contentLocator,
                            ICrowdfundingViewModelFactory viewModelFactory,
                            IContributionRepository contributionRepository)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.ViewCampaign);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.ViewCampaign);
        var campaignId = int.Parse(match.Groups[1].Value);
        var campaign = ContentLocator.ById<CampaignContent>(campaignId);

        var contributions = await _contributionRepository.FindByCampaignAsync(campaign.Content().Key);

        return await ViewCampaignViewModel.ForAsync(ViewModelFactory,
                                                    ContentLocator,
                                                    this,
                                                    campaign,
                                                    contributions);
    }
    
    public static string Url(IContentLocator contentLocator, Guid campaignKey) {
        var campaign = contentLocator.ById<CampaignContent>(campaignKey);
        
        return GenerateUrl(contentLocator, campaign.Content().RelativeUrl());
    }
}
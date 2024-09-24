using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewCampaignPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ILookups _lookups;

    public ViewCampaignPage(IContentLocator contentLocator,
                            ICrowdfundingViewModelFactory viewModelFactory,
                            IContributionRepository contributionRepository,
                            ILookups lookups)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _lookups = lookups;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewCampaign);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewCampaign);
        var campaignId = int.Parse(match.Groups[1].Value);
        var campaign = ContentLocator.ById<CampaignContent>(campaignId);
        var campaignFundraisers = GetFundraisersContent(campaign.CampaignId);
        
        var campaignContributions = await _contributionRepository.FindByCampaignAsync(campaign.Content().Key);
        var fundraiserContributions = await GetFundraiserContributions(campaignFundraisers);

        return await ViewCampaignViewModel.ForAsync(ViewModelFactory,
                                                    ContentLocator,
                                                    _lookups,
                                                    this,
                                                    query,
                                                    campaign,
                                                    campaignContributions,
                                                    fundraiserContributions,
                                                    campaignFundraisers);
    }
    
    public static string Url(IContentLocator contentLocator, Guid campaignKey) {
        var campaign = contentLocator.ById<CampaignContent>(campaignKey);
        
        var relativeUrl = new Url(campaign.Content().RelativeUrl());
        
        return GenerateUrl(contentLocator, CrowdfundingConstants.Routes.ViewCampaign_2.FormatWith(campaign.Content().Id,
                                                                                                  relativeUrl.PathSegments.Last()));
    }

    private List<FundraiserContent> GetFundraisersContent(Guid campaignId) {
        var campaignFundraisers = ContentLocator.All<FundraiserContent>().Where(x => x.Campaign.Id == campaignId);
        
        return campaignFundraisers.ToList();
    }
    
    private async Task<IReadOnlyList<Contribution>> GetFundraiserContributions(IReadOnlyList<FundraiserContent> fundraisers) {
        if (fundraisers.None()) {
            return new List<Contribution>();
        }
        
        var fundraisersIds = fundraisers.Select(x => x.FundraiserId.GetValueOrThrow()).ToArray();
        
        var contributions = await _contributionRepository.FindByFundraiserAsync(fundraisersIds.ToArray());
        
        return contributions;
    }
}
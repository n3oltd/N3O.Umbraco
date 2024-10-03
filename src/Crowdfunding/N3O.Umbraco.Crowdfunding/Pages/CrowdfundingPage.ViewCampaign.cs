using Flurl;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Entities;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.OpenGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class ViewCampaignPage : CrowdfundingPage {
    private readonly IContributionRepository _contributionRepository;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IForexConverter _forexConverter;
    private readonly ILookups _lookups;

    public ViewCampaignPage(IContentLocator contentLocator,
                            ICrowdfundingViewModelFactory viewModelFactory,
                            IContributionRepository contributionRepository,
                            ICurrencyAccessor currencyAccessor,
                            IForexConverter forexConverter,
                            ILookups lookups)
        : base(contentLocator, viewModelFactory) {
        _contributionRepository = contributionRepository;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
        _lookups = lookups;
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        if (!IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewCampaign)) {
            return false;
        }
        
        var campaign = GetCampaign(crowdfundingPath);

        if (campaign == null || campaign.Status == CrowdfunderStatuses.Draft) {
            return false;
        }
        
        return true;
    }
    
    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        var campaign = GetCampaign(crowdfundingPath);

        builder.WithTitle(campaign.Name);
        builder.WithDescription(campaign.Description);
        builder.WithUrl(Url(ContentLocator, campaign.CampaignId));
        builder.WithImagePath(campaign.OpenGraphImagePath);
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var campaign = GetCampaign(crowdfundingPath);
        var campaignFundraisers = GetFundraisersContent(campaign.CampaignId);
        
        var campaignContributions = await _contributionRepository.FindByCampaignAsync(campaign.Content().Key);
        var fundraiserContributions = await GetFundraiserContributions(campaignFundraisers);

        return await ViewCampaignViewModel.ForAsync(ViewModelFactory,
                                                    ContentLocator,
                                                    _currencyAccessor,
                                                    _forexConverter,
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
    
    private CampaignContent GetCampaign(string crowdfundingPath) {
        var match = Match(crowdfundingPath, CrowdfundingConstants.Routes.Patterns.ViewCampaign);
        var campaignId = int.Parse(match.Groups[1].Value);
        var campaign = ContentLocator.ById<CampaignContent>(campaignId);

        return campaign;
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
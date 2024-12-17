using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.OpenGraph;
using Smidge;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding;

public class CreateFundraiserPage : CrowdfundingPage {
    private readonly ILookups _lookups;
    private readonly IForexConverter _forexConverter;

    public CreateFundraiserPage(IContentLocator contentLocator,
                                IFormatter formatter,
                                ICrowdfundingUrlBuilder urlBuilder,
                                ICrowdfundingViewModelFactory viewModelFactory,
                                ILookups lookups,
                                IForexConverter forexConverter)
        : base(contentLocator, formatter, urlBuilder, viewModelFactory) {
        _lookups = lookups;
        _forexConverter = forexConverter;
    }

    public override void AddAssets(ISmidgeRequire bundle) {
        bundle.RequiresJs("~/assets/js/create-fundraiser-page.js");
    }

    protected override void AddOpenGraph(IOpenGraphBuilder builder,
                                         string crowdfundingPath,
                                         IReadOnlyDictionary<string, string> query) {
        // TODO
    }
    
    protected override string GetPageTitle(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        return Formatter.Text.Format<Strings>(s => s.PageTitle);
    }

    protected override bool IsMatch(string crowdfundingPath, IReadOnlyDictionary<string, string> query) {
        if (IsMatch(crowdfundingPath, CrowdfundingConstants.Routes.CreateFundraiser)) {
            var campaignContent = GetCampaignContent(query);

            if (campaignContent.HasValue()) {
                return true;
            }
        }

        return false;
    }

    protected override async Task<ICrowdfundingViewModel> GetViewModelAsync(string crowdfundingPath,
                                                                            IReadOnlyDictionary<string, string> query) {
        var campaignContent = GetCampaignContent(query);
        
        return await CreateFundraiserViewModel.ForAsync(ViewModelFactory,
                                                        _forexConverter,
                                                        _lookups,
                                                        this,
                                                        query,
                                                        campaignContent);
    }

    public override PageAccess Access => PageAccesses.SignedInWithAccount;

    private CampaignContent GetCampaignContent(IReadOnlyDictionary<string, string> query) {
        var campaignId = query[Parameters.CampaignId].TryParseAs<Guid>();
        var content = campaignId.IfNotNull(x => ContentLocator.ById(x));

        if (content?.ContentType.Alias.EqualsInvariant(CrowdfundingConstants.Campaign.Alias) == true) {
            return content.As<CampaignContent>();
        } else {
            return null;
        }
    }
    
    public static string Url(ICrowdfundingUrlBuilder urlBuilder, Guid campaignKey) {
        return urlBuilder.GenerateUrl(CrowdfundingConstants.Routes.CreateFundraiser,
                                      url => url.SetQueryParam(Parameters.CampaignId, campaignKey));
    }
    
    private static class Parameters {
        public static readonly string CampaignId = "campaignId";
    }

    public class Strings : CodeStrings {
        public string PageTitle => "Create a Fundraising Page";
    }
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Fundraiser.Alias)]
public class FundraiserContent : CrowdfunderContent<FundraiserContent>, IFundraiser {
    public string Account => GetValue(x => x.Account);
    public string Slug => GetValue(x => x.Slug);
    public CampaignContent Campaign => GetPickedAs(x => x.Campaign);
    public IPublishedContent Owner => GetPickedAs(x => x.Owner);
    
    public override Guid CampaignId => Campaign.Key;
    public override string CampaignName => Campaign.Name;
    public override Guid? TeamId => null;
    public override string TeamName => null;
    public override Guid? FundraiserId => Key;
    
    public override string Url(IContentLocator contentLocator) {
        return ViewEditFundraiserPage.Url(contentLocator, Key);
    }
}
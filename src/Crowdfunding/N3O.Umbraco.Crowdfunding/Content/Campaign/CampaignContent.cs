using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Campaign.Alias)]
public class CampaignContent : CrowdfunderContent<CampaignContent>, ICampaign {
    public IEnumerable<IPublishedContent> Tags => GetPickedAs(x => x.Tags);
    
    public override Guid CampaignId => Key;
    public override string CampaignName => Name;
    public override Guid? TeamId => null;
    public override string TeamName => null;
    public override Guid? FundraiserId => null;

    public override string Url(IContentLocator contentLocator) {
        return ViewCampaignPage.Url(contentLocator, Key);
    }
}
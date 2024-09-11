using N3O.Umbraco.Attributes;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Campaign.Alias)]
public class CampaignContent : CrowdfunderContent<CampaignContent> {
    public IEnumerable<IPublishedContent> Tags => GetPickedAs(x => x.Tags);
}
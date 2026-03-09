using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.CrowdfundingCampaign.CompositionAlias)]
public class CrowdfundingCampaignContent : UmbracoContent<CrowdfundingCampaignContent> {
    public string Name => Content().Name;
    public Guid Key => Content().Key;
    
    public bool CrowdfundingEnabled => GetValue(x => x.CrowdfundingEnabled);
}
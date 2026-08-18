using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

// TODO Delete along with the composition once every site has been migrated. Only the migration reads it.
[UmbracoContent(PlatformsConstants.CrowdfundingCampaign.CompositionAlias)]
public class CrowdfundingCampaignContent : UmbracoContent<CrowdfundingCampaignContent> {
    public Guid Key => Content().Key;
    
    public bool CrowdfundingEnabled => GetValue(x => x.CrowdfundingEnabled);
}
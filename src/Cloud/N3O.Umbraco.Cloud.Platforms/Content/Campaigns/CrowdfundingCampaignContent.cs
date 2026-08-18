using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

// TODO Delete this along with the composition once every site has been migrated. It models the crowdfunding tab
// campaigns carried before crowdfunders became their own document type, and is retained only so the migration can
// find campaigns with crowdfunding enabled and read the content it moves onto the crowdfunder.
[UmbracoContent(PlatformsConstants.CrowdfundingCampaign.CompositionAlias)]
public class CrowdfundingCampaignContent : UmbracoContent<CrowdfundingCampaignContent> {
    public Guid Key => Content().Key;
    
    public bool CrowdfundingEnabled => GetValue(x => x.CrowdfundingEnabled);
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.RegularGiving)]
public class RegularGivingCampaignContent : UmbracoContent<RegularGivingCampaignContent> {
    public RegularGivingFrequency RegularGivingFrequency => GetValue(x => x.RegularGivingFrequency);
}
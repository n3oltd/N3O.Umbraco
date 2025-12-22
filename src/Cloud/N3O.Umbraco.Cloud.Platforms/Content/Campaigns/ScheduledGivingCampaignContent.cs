using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.ScheduledGiving)]
public class ScheduledGivingCampaignContent : UmbracoContent<ScheduledGivingCampaignContent> {
    public GivingSchedule Schedule => GetValue(x => x.Schedule);
}
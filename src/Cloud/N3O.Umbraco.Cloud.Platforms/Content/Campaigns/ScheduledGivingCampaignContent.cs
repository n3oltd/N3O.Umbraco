using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.ScheduledGiving)]
public class ScheduledGivingCampaignContent : UmbracoContent<StandardCampaignContent> { }
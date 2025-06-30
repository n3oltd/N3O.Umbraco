using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Campaigns.Telethon)]
public class TelethonCampaignContent : UmbracoContent<TelethonCampaignContent> {
    public DateTime BeginAt => GetValue(x => x.BeginAt);
    public DateTime EndAt => GetValue(x => x.EndAt);
}
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class TelethonCampaignContent : UmbracoContent<TelethonCampaignContent> {
    public DateTime BeginAt => GetValue(x => x.BeginAt);
    public DateTime EndAt => GetValue(x => x.EndAt);
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using NodaTime.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public PublishedCampaignMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaign>((_, _) => new PublishedCampaign(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, PublishedCampaign dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Image = _mediaUrl.GetMediaUrl(src.Image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.Icon = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.Designations = src.Designations.OrEmpty().Select(ctx.Map<DesignationContent, PublishedDesignation>).ToList();
        dest.Tags = src?.AnalyticsTags.ToPublishedAnalyticsParameters();
        
        if (src.Type == CampaignTypes.Telethon) {
            dest.Telethon = new PublishedTelethonCampaign();
            
            dest.Telethon.BeginAt = src.Telethon.BeginAt.ToInstant().ToString();
            dest.Telethon.EndAt = src.Telethon.EndAt.ToInstant().ToString();
        } else if (src.Type == CampaignTypes.ScheduledGiving) {
            dest.ScheduledGiving = new PublishedScheduledGivingCampaign();
        }
    }
}
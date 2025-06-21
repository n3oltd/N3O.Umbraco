using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using NodaTime.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaign>((_, _) => new PublishedCampaign(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, PublishedCampaign dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Image = src.Image.GetPublishedUri();
        dest.Icon = src.Icon.GetPublishedUri();
        dest.Designations = src.Designations.OrEmpty().Select(ctx.Map<DesignationContent, PublishedDesignation>).ToList();
        dest.Analytics = src?.AnalyticsTags.ToPublishedAnalyticsParameters();
        
        if (src.Type == CampaignTypes.Telethon) {
            dest.Telethon = new PublishedTelethonCampaign();
            
            dest.Telethon.BeginAt = src.Telethon.BeginAt.ToInstant().ToString();
            dest.Telethon.EndAt = src.Telethon.EndAt.ToInstant().ToString();
        } else if (src.Type == CampaignTypes.ScheduledGiving) {
            dest.ScheduledGiving = new PublishedScheduledGivingCampaign();   
        }
    }
}
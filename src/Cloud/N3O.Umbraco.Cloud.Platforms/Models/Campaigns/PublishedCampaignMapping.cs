using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using NodaTime.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;
    
    public PublishedCampaignMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaign>((_, _) => new PublishedCampaign(), MapPublishedCampaign);
        mapper.Define<CampaignContent, PublishedCampaignSummary>((_, _) => new PublishedCampaignSummary(), MapPublishedCampaignSummary);
    }
    
    // Umbraco.Code.MapAll
    private void MapPublishedCampaign(CampaignContent src, PublishedCampaign dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Name = src.Name;
        dest.Type = (CampaignType) Enum.Parse(typeof(CampaignType), src.Type.Id, true);
        dest.Icon = new Uri(src.Icon.GetCropUrl(urlMode: UrlMode.Absolute));
        dest.Designations = src.Designations.OrEmpty().Select(ctx.Map<DesignationContent, PublishedDesignation>).ToList();
        dest.Analytics = ctx.Map<IEnumerable<DataListItem>, PublishedAnalyticsParameters>(src.AnalyticsTags);
        
        if (src.Type == CampaignTypes.Telethon) {
            var telethon = new PublishedTelethonCampaign();
            
            telethon.BeginAt = src.Telethon.BeginAt.ToInstant().ToString();
            telethon.EndAt = src.Telethon.EndAt.ToInstant().ToString();
        }
        
        dest.ScheduledGiving = null;
    }
    
    private void MapPublishedCampaignSummary(CampaignContent src, PublishedCampaignSummary dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Name = src.Name;
        dest.Type = (CampaignType) Enum.Parse(typeof(CampaignType), src.Type.Id, true);
        dest.Image = new Uri(src.Image.GetCropUrl(urlMode: UrlMode.Absolute));
        dest.Icon = new Uri(src.Icon.GetCropUrl(urlMode: UrlMode.Absolute));
    }
}
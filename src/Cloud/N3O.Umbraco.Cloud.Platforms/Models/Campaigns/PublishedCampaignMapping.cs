using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Community.Contentment.DataEditors;

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
        var designations = src.Descendants()
                              .Where(x => x.IsComposedOf(AliasHelper<Designation>.ContentTypeAlias()))
                              .As<IDesignation>();
        
        dest.Id = src.Key.ToString();
        dest.Name = src.Name;
        dest.Type = GetCampaignType(src);
        dest.Image = new Uri(_urlBuilder.Root().AppendPathSegment(src.Image.SrcUrl()));
        dest.Icon = new Uri(_urlBuilder.Root().AppendPathSegment(src.Icon.SrcUrl()));
        dest.Designations = designations.OrEmpty().Select(ctx.Map<IDesignation, PublishedDesignation>).ToList();
        dest.Analytics = ctx.Map<IEnumerable<DataListItem>, PublishedAnalyticsParameters>(src.AnalyticsTags);
        dest.Telethon = GetTelethonCampaign(src);
        
        dest.ScheduledGiving = null;
    }
    
    private void MapPublishedCampaignSummary(CampaignContent src, PublishedCampaignSummary dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Name = src.Name;
        dest.Type = GetCampaignType(src);
        dest.Image = new Uri(_urlBuilder.Root().AppendPathSegment(src.Image.SrcUrl()));
        dest.Icon = new Uri(_urlBuilder.Root().AppendPathSegment(src.Icon.SrcUrl()));
    }

    private PublishedTelethonCampaign GetTelethonCampaign(CampaignContent src) {
        if (src is TelethonCampaign telethonCampaign) {
            var telethon = new PublishedTelethonCampaign();
            telethon.BeginAt = telethonCampaign.BeginAt.ToInstant().ToString();
            telethon.EndAt = telethonCampaign.EndAt.ToInstant().ToString();
            
            return telethon;
        }
        
        return null;
    }

    private CampaignType GetCampaignType(ICampaign campaign) {
        if (campaign is StandardCampaign) {
            return CampaignType.Standard;
        } else if (campaign is TelethonCampaign) {
            return CampaignType.Telethon;
        } else {
            throw UnrecognisedValueException.For(campaign.ContentType.Alias);
        }
    }
}
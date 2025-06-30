using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignSummaryMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public PublishedCampaignSummaryMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaignSummary>((_, _) => new PublishedCampaignSummary(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, PublishedCampaignSummary dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Image = _mediaUrl.GetMediaUrl(src.Image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.Icon = _mediaUrl.GetMediaUrl(src.Icon, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
    }
}
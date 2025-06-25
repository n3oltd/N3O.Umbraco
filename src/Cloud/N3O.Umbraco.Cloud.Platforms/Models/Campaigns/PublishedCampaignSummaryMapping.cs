using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignSummaryMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PublishedCampaignSummaryMapping(IContentLocator contentLocator, IWebHostEnvironment webHostEnvironment) {
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaignSummary>((_, _) => new PublishedCampaignSummary(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, PublishedCampaignSummary dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Image = src.Image.GetPublishedUri(_contentLocator, _webHostEnvironment);
        dest.Icon = src.Icon.GetPublishedUri(_contentLocator, _webHostEnvironment);
    }
}
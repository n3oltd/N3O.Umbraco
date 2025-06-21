using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;
using CampaignType = N3O.Umbraco.Cloud.Platforms.Clients.CampaignType;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCampaignSummaryMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CampaignContent, PublishedCampaignSummary>((_, _) => new PublishedCampaignSummary(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(CampaignContent src, PublishedCampaignSummary dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = src.Type.ToEnum<CampaignType>();
        dest.Name = src.Name;
        dest.Image = src.Image.GetPublishedUri();
        dest.Icon = src.Icon.GetPublishedUri();
    }
}
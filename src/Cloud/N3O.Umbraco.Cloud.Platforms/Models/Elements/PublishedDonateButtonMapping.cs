using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDonateButtonMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public PublishedDonateButtonMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, PublishedDonateButton>((_, _) => new PublishedDonateButton(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, PublishedDonateButton dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = ElementType.DonateButton;
        dest.Label = src.DonateButton.Label;

        if (src.Campaign.HasValue()) {
            dest.Campaign = ctx.Map<CampaignContent, PublishedCampaignSummary>(src.Campaign);
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(src.Campaign.DefaultDesignation);
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(defaultCampaign.DefaultDesignation);
        }
        
        dest.Tags = src.Tags.ToPublishedTagCollection();
    }
}
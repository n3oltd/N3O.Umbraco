using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using PlatformsDonateButtonAction = N3O.Umbraco.Cloud.Platforms.Clients.DonateButtonAction;

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
        dest.System = src.IsSystemGenerated;
        dest.Text = src.DonateButton.Text;
        dest.Amount = (double) src.DonateButton.Amount;
        dest.Action = src.DonateButton.Action.ToEnum<PlatformsDonateButtonAction>();;

        if (src.Campaign.HasValue()) {
            dest.Campaign = ctx.Map<CampaignContent, PublishedCampaignSummary>(src.Campaign);
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(src.Campaign.DefaultDesignation);
        } else if (src.DonateButton.HasValue(x => x.Designation)) {
            var campaign = src.DonateButton.Designation.Content().Parent.As<CampaignContent>();
            
            dest.Campaign = ctx.Map<CampaignContent, PublishedCampaignSummary>(campaign);
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(src.DonateButton.Designation);
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(defaultCampaign.DefaultDesignation);
        }

        dest.Dimension1 = src.DonateButton.Dimension1?.Name;
        dest.Dimension2 = src.DonateButton.Dimension2?.Name;
        dest.Dimension3 = src.DonateButton.Dimension3?.Name;
        dest.Dimension4 = src.DonateButton.Dimension4?.Name;
        
        dest.Tags = src.Tags.ToPublishedTagCollection();
    }
}
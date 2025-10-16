using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedDonationFormMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;

    public PublishedDonationFormMapping(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ElementContent, PublishedDonationForm>((_, _) => new PublishedDonationForm(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(ElementContent src, PublishedDonationForm dest, MapperContext ctx) {
        dest.Id = src.Key.ToString();
        dest.Type = ElementType.DonationForm;
        dest.System = src.IsSystemGenerated;

        if (src.Campaign.HasValue()) {
            dest.Campaign = ctx.Map<CampaignContent, PublishedCampaignSummary>(src.Campaign);
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(src.Campaign.DefaultDesignation);
        } else if (src.DonationForm.HasValue(x => x.Designation)) {
            var campaign = src.DonationForm.Designation.Content().Parent.As<CampaignContent>();
            
            dest.Campaign = ctx.Map<CampaignContent, PublishedCampaignSummary>(campaign);
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(src.DonationForm.Designation);
        } else {
            var defaultCampaign = _contentLocator.Single<PlatformsContent>().Campaigns.First();
            
            dest.Designation = ctx.Map<DesignationContent, PublishedDesignation>(defaultCampaign.DefaultDesignation);
        }
        
        dest.Dimension1 = src.DonationForm.Dimension1?.Name;
        dest.Dimension2 = src.DonationForm.Dimension3?.Name;
        dest.Dimension3 = src.DonationForm.Dimension3?.Name;
        dest.Dimension4 = src.DonationForm.Dimension4?.Name;
        
        dest.Tags = src.Tags.ToPublishedTagCollection();
    }
}
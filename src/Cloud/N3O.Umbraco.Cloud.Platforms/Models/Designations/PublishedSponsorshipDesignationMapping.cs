using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedSponsorshipDesignationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDesignationContent, PublishedSponsorshipDesignation>((_, _) => new PublishedSponsorshipDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(SponsorshipDesignationContent src, PublishedSponsorshipDesignation dest, MapperContext ctx) {
        dest.Scheme = new PublishedSponsorshipScheme();
        dest.Scheme.Id = src.Scheme.Name;
        dest.Components = src.Scheme.Components.OrEmpty().Select(x => ToPublishedSponsorshipComponent(ctx, x)).ToList();
        dest.AllowedDurations = src.Scheme.AllowedDurations.OrEmpty().Select(ToPublishedCommitmentDuration).ToList();
    }
    
    private PublishedSponsorshipComponent ToPublishedSponsorshipComponent(MapperContext ctx, SponsorshipComponent src) {
        var component = new PublishedSponsorshipComponent();
        
        component.Name = src.Name;
        component.Mandatory = src.Mandatory;
        
        if (src.HasPricing()) {
            component.Pricing = ctx.Map<IPricing, PublishedPricing>(src);
        }
        
        return component;
    }

    private PublishedCommitmentDuration ToPublishedCommitmentDuration(SponsorshipDuration sponsorshipDuration) {
        var commitmentDuration = new PublishedCommitmentDuration();
        commitmentDuration.Id = sponsorshipDuration.Id;
        commitmentDuration.Name = sponsorshipDuration.Name;
        commitmentDuration.Months = sponsorshipDuration.Months;
        
        return commitmentDuration;
    }
}

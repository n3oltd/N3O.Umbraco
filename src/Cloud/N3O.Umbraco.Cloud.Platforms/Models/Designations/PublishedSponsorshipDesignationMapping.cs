using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedSponsorshipDesignationMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public PublishedSponsorshipDesignationMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDesignationContent, PublishedSponsorshipDesignation>((_, _) => new PublishedSponsorshipDesignation(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(SponsorshipDesignationContent src, PublishedSponsorshipDesignation dest, MapperContext ctx) {
        var sponsorshipScheme = src.Scheme;
        
        dest.Scheme = new PublishedSponsorshipScheme();
        dest.Scheme.Id = sponsorshipScheme.Id;
        dest.Components = sponsorshipScheme.Components.OrEmpty().Select(x => ToPublishedSponsorshipComponent(ctx, x)).ToList();
        dest.AllowedDurations = sponsorshipScheme.AllowedDurations.OrEmpty().Select(ToPublishedCommitmentDuration).ToList();
    }
    
    private PublishedSponsorshipComponent ToPublishedSponsorshipComponent(MapperContext ctx, SponsorshipComponent src) {
        var component = new PublishedSponsorshipComponent();
        
        component.Name = src.Name;
        component.Mandatory = src.Mandatory;
        component.Pricing = src.Pricing.IfNotNull(ctx.Map<IPricing, PublishedPricing>);
        
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

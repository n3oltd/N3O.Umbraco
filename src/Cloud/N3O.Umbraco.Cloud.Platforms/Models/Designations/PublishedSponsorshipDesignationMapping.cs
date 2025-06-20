using Humanizer;
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

    protected void Map(SponsorshipDesignationContent src, PublishedSponsorshipDesignation dest, MapperContext ctx) {
        dest.Scheme = new PublishedSponsorshipScheme();
        dest.Scheme.Id = src.Scheme.Name.Camelize();
        dest.Components = src.Scheme.Components.OrEmpty().Select(x => GetComponent(ctx, x)).ToList();
        dest.AllowedDurations = src.Scheme.AllowedDurations.OrEmpty().Select(GetAllowedDuration).ToList();
    }

    private PublishedCommitmentDuration GetAllowedDuration(SponsorshipDuration sponsorshipDuration) {
        var commitmentDuration = new PublishedCommitmentDuration();
        commitmentDuration.Id = sponsorshipDuration.Id;
        commitmentDuration.Name = sponsorshipDuration.Name;
        commitmentDuration.Months = sponsorshipDuration.Months;
        
        return commitmentDuration;
    }

    private PublishedSponsorshipComponent GetComponent(MapperContext ctx, SponsorshipComponent src) {
        var component = new PublishedSponsorshipComponent();
        
        component.Name = src.Name;
        component.Mandatory = src.Mandatory;
        
        if (src.HasPricing()) {
            component.Pricing = new PublishedPricing();
            component.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(src);
            component.Pricing.Rules = src.PriceRules.OrEmpty().Select(ctx.Map<IPricingRule, PublishedPricingRule>).ToList();
        }
        
        return component;
    }
}

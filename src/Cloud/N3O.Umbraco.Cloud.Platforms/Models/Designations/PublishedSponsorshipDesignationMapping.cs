using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedSponsorshipDesignationMapping : PublishedDesignationTypeMapping<PublishedSponsorshipDesignation> {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDesignation, PublishedSponsorshipDesignation>((_, _) => new PublishedSponsorshipDesignation(), Map);
    }

    protected override void Map(IDesignation src, PublishedSponsorshipDesignation dest, MapperContext ctx) {
        var sponsorshipDesignation = src as SponsorshipDesignation;
        var components = sponsorshipDesignation.Children.As<SponsorshipSchemeComponent>();
        
        dest.Scheme = new PublishedSponsorshipScheme();
        dest.Scheme.Id = sponsorshipDesignation.Scheme.Name.Camelize();
        dest.Components = components.OrEmpty().Select(x => GetComponent(ctx, x)).ToList();
        dest.AllowedDurations = sponsorshipDesignation.Scheme.AllowedDurations.OrEmpty().Select(GetAllowedDuration).ToList();
    }

    private PublishedCommitmentDuration GetAllowedDuration(SponsorshipDuration sponsorshipDuration) {
        var commitmentDuration = new PublishedCommitmentDuration();
        commitmentDuration.Id = sponsorshipDuration.Id;
        commitmentDuration.Name = sponsorshipDuration.Name;
        commitmentDuration.Months = sponsorshipDuration.Months;
        
        return commitmentDuration;
    }

    private PublishedSponsorshipComponent GetComponent(MapperContext ctx, SponsorshipSchemeComponent src) {
        var component = new PublishedSponsorshipComponent();
        
        component.Name = src.Name;
        component.Mandatory = src.Mandatory;
        
        if (HasPricing(src)) {
            component.Pricing = new PublishedPricing();
            component.Pricing.Price = ctx.Map<IPrice, PublishedPrice>(src);
            component.Pricing.Rules = src.PriceRules.OrEmpty().Select(ctx.Map<PricingRule, PublishedPricingRule>).ToList();
        }
        
        return component;
    }
}

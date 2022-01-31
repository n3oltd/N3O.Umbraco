using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Giving.Sponsorships.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Sponsorships.Models {
    public class SponsorshipSchemeComponentMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<SponsorshipSchemeComponent, SponsorshipSchemeComponentRes>((_, _) => new SponsorshipSchemeComponentRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(SponsorshipSchemeComponent src, SponsorshipSchemeComponentRes dest, MapperContext ctx) {
            ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
            dest.Mandatory = src.Mandatory;
            dest.Pricing = ctx.Map<IPricing, PricingRes>(src);
        }
    }
}
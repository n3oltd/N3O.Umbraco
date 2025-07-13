using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class DonationItemMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationItem, DonationItemRes>((_, _) => new DonationItemRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(DonationItem src, DonationItemRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.AllowedGivingTypes = src.AllowedGivingTypes;
        dest.FundDimensionOptions = ctx.Map<IFundDimensionOptions, FundDimensionOptionsRes>(src.FundDimensionOptions);
        dest.Pricing = src.Pricing.IfNotNull(ctx.Map<IPricing, PricingRes>);
    }
}

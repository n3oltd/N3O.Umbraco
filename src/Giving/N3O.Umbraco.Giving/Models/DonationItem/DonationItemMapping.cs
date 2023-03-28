using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class DonationItemMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<DonationItem, DonationItemRes>((_, _) => new DonationItemRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(DonationItem src, DonationItemRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.AllowedGivingTypes = src.AllowedGivingTypes;
        dest.Dimension1Options = src.Dimension1Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension1Value, FundDimensionValueRes>)
                                    .ToList();
        dest.Dimension2Options = src.Dimension2Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension2Value, FundDimensionValueRes>)
                                    .ToList();
        dest.Dimension3Options = src.Dimension3Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension3Value, FundDimensionValueRes>)
                                    .ToList();
        dest.Dimension4Options = src.Dimension4Options
                                    .OrEmpty()
                                    .Select(ctx.Map<FundDimension4Value, FundDimensionValueRes>)
                                    .ToList();
        dest.Pricing = ctx.Map<IPricing, PricingRes>(src);
    }
}

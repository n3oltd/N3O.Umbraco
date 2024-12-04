using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipSchemeMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipScheme, SponsorshipSchemeRes>((_, _) => new SponsorshipSchemeRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(SponsorshipScheme src, SponsorshipSchemeRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.AllowedGivingTypes = src.AllowedGivingTypes;
        dest.AllowedDurations = src.AllowedDurations;
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
        dest.Components = src.Components.OrEmpty().Select(ctx.Map<SponsorshipComponent, SponsorshipComponentRes>);
    }
}

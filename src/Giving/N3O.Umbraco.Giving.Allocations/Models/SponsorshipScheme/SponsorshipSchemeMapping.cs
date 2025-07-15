using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipSchemeMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipScheme, SponsorshipSchemeRes>((_, _) => new SponsorshipSchemeRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(SponsorshipScheme src, SponsorshipSchemeRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);

        dest.AllowedGivingTypes = src.AllowedGivingTypes;
        dest.AllowedDurations = src.AllowedDurations;
        dest.FundDimensionOptions = ctx.Map<IFundDimensionOptions, FundDimensionOptionsRes>(src.FundDimensionOptions);
        dest.Components = src.Components.OrEmpty().Select(ctx.Map<SponsorshipComponent, SponsorshipComponentRes>);
    }
}

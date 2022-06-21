using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipComponentAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipComponentAllocation, SponsorshipComponentAllocationRes>((_, _) => new SponsorshipComponentAllocationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipComponentAllocation src, SponsorshipComponentAllocationRes dest, MapperContext ctx) {
        dest.Component = src.Component;
        dest.Value = ctx.Map<Money, MoneyRes>(src.Value);
    }
}

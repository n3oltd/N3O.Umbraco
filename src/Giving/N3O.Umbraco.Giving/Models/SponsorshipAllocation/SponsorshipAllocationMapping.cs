using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipAllocation, SponsorshipAllocationRes>((_, _) => new SponsorshipAllocationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipAllocation src, SponsorshipAllocationRes dest, MapperContext ctx) {
        dest.BeneficiaryReference = src.BeneficiaryReference;
        dest.Scheme = src.Scheme;
        dest.Duration = src.Duration;
        dest.Components = src.Components
                             .OrEmpty()
                             .Select(ctx.Map<SponsorshipComponentAllocation, SponsorshipComponentAllocationRes>)
                             .ToList();
    }
}

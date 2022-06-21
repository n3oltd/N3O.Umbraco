using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FundAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundAllocation, FundAllocationRes>((_, _) => new FundAllocationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FundAllocation src, FundAllocationRes dest, MapperContext ctx) {
        dest.DonationItem = src.DonationItem;
    }
}

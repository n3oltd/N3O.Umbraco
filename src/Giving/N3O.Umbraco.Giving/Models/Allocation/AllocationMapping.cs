using N3O.Giving.Models;
using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models {
    public class AllocationMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Allocation, AllocationRes>((_, _) => new AllocationRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Allocation src, AllocationRes dest, MapperContext ctx) {
            dest.Type = src.Type;
            dest.Value = ctx.Map<Money, MoneyRes>(src.Value);
            dest.FundDimensions = ctx.Map<FundDimensionValues, FundDimensionValuesRes>(src.FundDimensions);
            dest.Fund = ctx.Map<FundAllocation, FundAllocationRes>(src.Fund);
            dest.Sponsorship = ctx.Map<SponsorshipAllocation, SponsorshipAllocationRes>(src.Sponsorship);
        }
    }
}
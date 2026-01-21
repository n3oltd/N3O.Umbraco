using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

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
        dest.Feedback = ctx.Map<FeedbackAllocation, FeedbackAllocationRes>(src.Feedback);
        dest.PledgeUrl = src.PledgeUrl;
        dest.UpsellOfferId = src.UpsellOfferId;
        dest.Upsell = src.UpsellOfferId.HasValue();
        dest.Notes = src.Notes;
        // TODO We may need to extend this so we resolve in the constructor of this mapper
        // IEnumerable<IAllocationExtensionMapping> and for each (key, value) in the dictionary
        // we find the corresponding mapping (if any) and invoke it to convert the JToken value
        // to the response model.
        dest.Extensions = src.Extensions;
    }
}

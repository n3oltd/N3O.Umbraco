using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackComponentAllocation, FeedbackComponentAllocationRes>((_, _) => new FeedbackComponentAllocationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackComponentAllocation src, FeedbackComponentAllocationRes dest, MapperContext ctx) {
        dest.Component = src.Component;
        dest.Value = ctx.Map<Money, MoneyRes>(src.Value);
    }
}

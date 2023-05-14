using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackAllocation, FeedbackAllocationRes>((_, _) => new FeedbackAllocationRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackAllocation src, FeedbackAllocationRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
        dest.Components = src.Components
                             .OrEmpty()
                             .Select(ctx.Map<FeedbackComponentAllocation, FeedbackComponentAllocationRes>)
                             .ToList();
    }
}

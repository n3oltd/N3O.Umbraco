using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackAllocation, FeedbackAllocationRes>((_, _) => new FeedbackAllocationRes(), Map);
    }

    private void Map(FeedbackAllocation src, FeedbackAllocationRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
    }
}

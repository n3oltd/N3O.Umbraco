using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackAllocation, FeedbackAllocationRes>((_, _) => new FeedbackAllocationRes(), Map);
    }

    private void Map(FeedbackAllocation src, FeedbackAllocationRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
        dest.CustomFields = src.CustomFields.OrEmpty().Select(ctx.Map<IFeedbackCustomField, FeedbackCustomFieldRes>);
    }
}

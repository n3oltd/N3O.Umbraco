using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeedbackGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackGoalElement, FeedbackGoalRes>((_, _) => new FeedbackGoalRes(), Map);
    }

    private void Map(FeedbackGoalElement src, FeedbackGoalRes dest, MapperContext ctx) {
        dest.Feedback = src.CustomFields
                           .OrEmpty()
                           .Select(ctx.Map<IFeedbackCustomField, FeedbackCustomFieldRes>)
                           .ToList();
    }
}
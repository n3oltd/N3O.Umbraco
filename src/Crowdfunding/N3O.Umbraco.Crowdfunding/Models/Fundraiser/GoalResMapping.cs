using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GoalElement, GoalRes>((_, _) => new GoalRes(), Map);
    }

    private void Map(GoalElement src, GoalRes dest, MapperContext ctx) {
        dest.OptionId = src.OptionId;
        dest.Value = src.Amount;
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackGoalElement, FeedbackGoalRes>);
    }
}
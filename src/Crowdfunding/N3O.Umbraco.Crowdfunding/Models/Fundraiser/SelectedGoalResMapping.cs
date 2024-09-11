using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SelectedGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GoalElement, SelectedGoalRes>((_, _) => new SelectedGoalRes(), Map);
    }

    private void Map(GoalElement src, SelectedGoalRes dest, MapperContext ctx) {
        dest.CampaignGoalId = src.GoalId;
        dest.Value = src.Amount;
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackGoalElement, SelectedFeedbackGoalRes>);
    }
}
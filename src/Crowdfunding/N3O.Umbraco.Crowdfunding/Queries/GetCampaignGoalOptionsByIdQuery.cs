using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetCampaignGoalOptionsByIdQuery : Request<None, GoalOptionRes> {
    public ContentId ContentId { get; }
    public GoalId GoalId { get; }

    public GetCampaignGoalOptionsByIdQuery(ContentId contentId, GoalId goalId) {
        ContentId = contentId;
        GoalId = goalId;
    }
}
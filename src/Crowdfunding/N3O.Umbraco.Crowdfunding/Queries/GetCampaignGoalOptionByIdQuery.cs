using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetCampaignGoalOptionByIdQuery : Request<None, GoalOptionRes> {
    public CampaignId CampaignId { get; }
    public GoalOptionId GoalOptionId { get; }

    public GetCampaignGoalOptionByIdQuery(CampaignId campaignId, GoalOptionId goalOptionId) {
        CampaignId = campaignId;
        GoalOptionId = goalOptionId;
    }
}
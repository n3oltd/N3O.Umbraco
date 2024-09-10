using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SelectedGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundraiserGoalElement, SelectedGoalRes>((_, _) => new SelectedGoalRes(), Map);
    }

    private void Map(FundraiserGoalElement src, SelectedGoalRes dest, MapperContext ctx) {
        dest.CampaignGoalId = src.CampaignGoalId;
        dest.Value = src.Amount;
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<CrowdfunderFeedbackGoalElement, SelectedFeedbackGoalRes>);
    }
}
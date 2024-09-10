using N3O.Umbraco.Crowdfunding.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserSelectedGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundraiserGoalElement, FundraiserSelectedGoalRes>((_, _) => new FundraiserSelectedGoalRes(), Map);
    }

    private void Map(FundraiserGoalElement src, FundraiserSelectedGoalRes dest, MapperContext ctx) {
        dest.GoalId = src.CampaignGoalID;
        dest.Value = src.Amount;
    }
}
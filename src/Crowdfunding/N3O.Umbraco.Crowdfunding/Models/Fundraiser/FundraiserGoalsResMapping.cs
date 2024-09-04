using N3O.Umbraco.Crowdfunding.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundraiserContent, FundraiserGoalsRes>((_, _) => new FundraiserGoalsRes(), Map);
    }

    private void Map(FundraiserContent src, FundraiserGoalsRes dest, MapperContext ctx) {
        dest.Goals = src.Campaign.Goals.Select(ctx.Map<CampaignGoalElement, FundraiserGoalRes>);
        dest.SelectedGoals = src.Goals.Select(ctx.Map<FundraiserGoalElement, FundraiserSelectedGoalRes>);
    }
}
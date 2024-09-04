using N3O.Umbraco.Crowdfunding.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CrowdfundingGoalElement, FundraiserGoalRes>((_, _) => new FundraiserGoalRes(), Map);
    }

    private void Map(CrowdfundingGoalElement src, FundraiserGoalRes dest, MapperContext ctx) {
        dest.GoalId = src.CampaignGoalID;
        dest.GoalName = src.Title;
        dest.Type = src.Type;
        dest.Tags = src.Tags.Select(x => x.Name);
    }
}
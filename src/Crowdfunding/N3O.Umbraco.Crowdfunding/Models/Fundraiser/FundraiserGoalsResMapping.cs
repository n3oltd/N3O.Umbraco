using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Financial;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FundraiserContent, FundraiserGoalsRes>((_, _) => new FundraiserGoalsRes(), Map);
    }

    private void Map(FundraiserContent src, FundraiserGoalsRes dest, MapperContext ctx) {
        dest.Currency = ctx.Map<Currency, CurrencyRes>(src.Currency);
        dest.Available = src.Campaign.Goals.Select(ctx.Map<GoalElement, AvailableGoalRes>);
        dest.Selected = src.Goals.Select(ctx.Map<GoalElement, SelectedGoalRes>);
    }
}
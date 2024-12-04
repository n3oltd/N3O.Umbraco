using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GoalElement, GoalRes>((_, _) => new GoalRes(), Map);
    }

    private void Map(GoalElement src, GoalRes dest, MapperContext ctx) {
        dest.OptionId = src.OptionId;
        dest.Value = src.Amount;
        dest.FundDimensions = ctx.Map<IFundDimensionValues, FundDimensionValuesRes>(src.FundDimensions);
        dest.Feedback = src.Feedback.IfNotNull(ctx.Map<FeedbackGoalElement, FeedbackGoalRes>);
        dest.Tags = src.Tags.OrEmpty().Select(ctx.Map<TagContent, TagRes>);
    }
}
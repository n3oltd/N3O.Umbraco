using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SelectedFeedbackGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackGoalElement, SelectedFeedbackGoalRes>((_, _) => new SelectedFeedbackGoalRes(), Map);
    }

    private void Map(FeedbackGoalElement src, SelectedFeedbackGoalRes dest, MapperContext ctx) {
        dest.Feedback = src.CustomFields
                           .OrEmpty()
                           .Select(ctx.Map<IFeedbackCustomField, FeedbackCustomFieldRes>)
                           .ToList();
    }
}
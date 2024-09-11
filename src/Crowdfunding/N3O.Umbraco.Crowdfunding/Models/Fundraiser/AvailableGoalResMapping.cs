using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class AvailableGoalResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GoalElement, AvailableGoalRes>((_, _) => new AvailableGoalRes(), Map);
    }

    private void Map(GoalElement src, AvailableGoalRes dest, MapperContext ctx) {
        dest.Id = src.GoalId;
        dest.Title = src.Title;
        dest.Type = src.Type;
        dest.Tags = src.Tags.Select(x => x.Name).ToList();
        dest.Fund = src.Fund.IfNotNull(x => ctx.Map<DonationItem, DonationItemRes>(x.DonationItem));
        dest.Feedback = src.Feedback.IfNotNull(x => ctx.Map<FeedbackScheme, FeedbackSchemeRes>(x.Scheme));
    }
}
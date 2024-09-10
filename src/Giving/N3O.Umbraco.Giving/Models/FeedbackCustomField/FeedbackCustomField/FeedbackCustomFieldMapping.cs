using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFeedbackCustomField, FeedbackCustomFieldRes>((_, _) => new FeedbackCustomFieldRes(), Map);
    }

    private void Map(IFeedbackCustomField src, FeedbackCustomFieldRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Alias = src.Alias;
        dest.Name = dest.Name;
        dest.Bool = src.Bool;
        dest.Date = src.Date;
        dest.Text = src.Text;
    }
}
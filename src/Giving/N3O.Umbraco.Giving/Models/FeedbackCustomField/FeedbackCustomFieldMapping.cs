using N3O.Umbraco.Giving.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackCustomFieldElement, FeedbackCustomFieldRes>((_, _) => new FeedbackCustomFieldRes(), Map);
    }

    private void Map(FeedbackCustomFieldElement src, FeedbackCustomFieldRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Value = src.Value;
    }
}
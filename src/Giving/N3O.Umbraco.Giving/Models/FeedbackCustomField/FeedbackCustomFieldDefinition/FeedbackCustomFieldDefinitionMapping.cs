using N3O.Umbraco.Giving.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldDefinitionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackCustomFieldElement, FeedbackCustomFieldDefinitionRes>((_, _) => new FeedbackCustomFieldDefinitionRes(), Map);
    }

    private void Map(FeedbackCustomFieldElement src, FeedbackCustomFieldDefinitionRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.TextMaxLength = src.TextMaxLength;
        dest.Required = src.Required;
    }
}
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackCustomFieldDefinitionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackCustomFieldDefinition, FeedbackCustomFieldDefinitionRes>((_, _) => new FeedbackCustomFieldDefinitionRes(), Map);
    }

    private void Map(FeedbackCustomFieldDefinition src, FeedbackCustomFieldDefinitionRes dest, MapperContext ctx) {
        dest.Type = src.Type;
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.TextMaxLength = src.TextMaxLength;
        dest.Required = src.Required;
    }
}
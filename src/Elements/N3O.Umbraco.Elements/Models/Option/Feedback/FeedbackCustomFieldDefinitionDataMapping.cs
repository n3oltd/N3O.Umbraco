using N3O.Umbraco.Giving.Allocations.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class FeedbackCustomFieldDefinitionDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackCustomFieldDefinitionElement, FeedbackCustomFieldDefinitionData>((_, _) => new FeedbackCustomFieldDefinitionData(), Map);
    }

    private void Map(FeedbackCustomFieldDefinitionElement src, FeedbackCustomFieldDefinitionData dest, MapperContext ctx) {
        dest.TypeId = src.Type.Id;
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.TextMaxLength = src.TextMaxLength;
        dest.Required = src.Required;
    }
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCustomFieldDefinitionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFeedbackCustomFieldDefinition, PublishedFeedbackCustomFieldDefinition>((_, _) => new PublishedFeedbackCustomFieldDefinition(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(IFeedbackCustomFieldDefinition src, PublishedFeedbackCustomFieldDefinition dest, MapperContext ctx) {
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.Required = src.Required;

        dest.Type = new PublishedFeedbackCustomFieldType();
        dest.Type.Id = src.Type.Id;
        dest.Type.Name = src.Type.Name;

        if (src.Type == FeedbackCustomFieldTypes.Text) {
            dest.Text = new PublishedFeedbackCustomFieldTextFieldOptions();
            dest.Text.MaxLength = src.TextMaxLength;
        }
    }
}
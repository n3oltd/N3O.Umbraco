using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedCustomFieldDefinitionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IFeedbackCustomFieldDefinition, PublishedCustomFieldDefinition>((_, _) => new PublishedCustomFieldDefinition(), Map);
    }
    
    // Umbraco.Code.MapAll
    private void Map(IFeedbackCustomFieldDefinition src, PublishedCustomFieldDefinition dest, MapperContext ctx) {
        dest.Alias = src.Alias;
        dest.Name = src.Name;
        dest.Type = src.Type.ToEnum<CustomFieldType>();
        dest.Required = src.Required;

        if (src.Type == FeedbackCustomFieldTypes.Text) {
            dest.Text = new PublishedTextFieldOptions();
            dest.Text.MaxLength = src.TextMaxLength;
        }
    }
}
using Humanizer;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Giving.Content;

public class FeedbackCustomFieldElement : UmbracoElement<FeedbackCustomFieldElement>, IFeedbackCustomFieldDefinition {
    [UmbracoProperty("fieldType")]
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);
    
    [UmbracoProperty("fieldName")]
    public string Name => GetValue(x => x.Name);
    
    [UmbracoProperty("fieldRequired")]
    public bool Required => GetValue(x => x.Required);
    
    [UmbracoProperty("fieldTextMaxLength")]
    public int? TextMaxLength => GetValue(x => x.TextMaxLength);
    
    public string Alias => Name.Camelize();
}
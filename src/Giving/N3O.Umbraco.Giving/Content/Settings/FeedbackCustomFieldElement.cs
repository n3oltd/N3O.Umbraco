using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Content; 

public class FeedbackCustomFieldElement : UmbracoElement<FeedbackCustomFieldElement> {
    [UmbracoProperty("fieldType")]
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);
    
    [UmbracoProperty("fieldValue")]
    public string Value => GetValue(x => x.Value);
}
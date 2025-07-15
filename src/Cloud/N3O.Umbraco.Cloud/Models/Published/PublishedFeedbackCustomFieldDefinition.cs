using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFeedbackCustomFieldDefinition : IFeedbackCustomFieldDefinition  {
    public string Alias { get; set; }
    public string Name { get; set; }
    public PublishedFeedbackCustomFieldType Type { get; set; }
    public bool Required { get; set; }
    public PublishedFeedbackCustomFieldTextFieldOptions Text { get; set; }
    
    [JsonIgnore]
    FeedbackCustomFieldType IFeedbackCustomFieldDefinition.Type => StaticLookups.FindById<FeedbackCustomFieldType>(Type.Id);
    
    [JsonIgnore]
    int? IFeedbackCustomFieldDefinition.TextMaxLength => Text.MaxLength;
}
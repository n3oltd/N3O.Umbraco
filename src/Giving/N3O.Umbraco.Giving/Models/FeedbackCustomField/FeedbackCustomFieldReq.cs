using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomFieldReq : IFeedbackCustomField {
    [Name("Type")]
    public FeedbackCustomFieldType Type { get; set; }
    
    [Name("Bool")]
    public bool? Bool { get; set; }
    
    [Name("Date")]
    public LocalDate? Date { get; set; }
    
    [Name("Text")]
    public string Text { get; set; }
}

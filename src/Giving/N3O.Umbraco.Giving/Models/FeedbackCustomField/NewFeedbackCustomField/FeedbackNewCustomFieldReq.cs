using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldReq : IFeedbackNewCustomField {
    [JsonConstructor]
    public FeedbackNewCustomFieldReq() { }

    public FeedbackNewCustomFieldReq(IFeedbackCustomField customField) {
        Alias = customField.Alias;
        Bool = customField.Bool;
        Date = customField.Date;
        Text = customField.Text;
    }

    [Name("Alias")]
    public string Alias { get; set; }

    [Name("Bool")]
    public bool? Bool { get; set; }
    
    [Name("Date")]
    public LocalDate? Date { get; set; }
    
    [Name("Text")]
    public string Text { get; set; }
}
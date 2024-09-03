using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeedbackGoalReq {
    [Name("Feedback Custom Fields")]
    public FeedbackNewCustomFieldsReq CustomFields { get; set; }
}
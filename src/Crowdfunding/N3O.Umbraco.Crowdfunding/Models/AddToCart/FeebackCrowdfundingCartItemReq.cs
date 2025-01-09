using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeebackCrowdfundingCartItemReq {
    [Name("Custom Fields")]
    public FeedbackNewCustomFieldsReq CustomFields { get; set; }
}
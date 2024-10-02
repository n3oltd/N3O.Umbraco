using CsvHelper.Configuration.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingCartItemReq {
    [Name("Goal ID")]
    public string GoalId { get; set; }

    [Name("Value")]
    public MoneyReq Value { get; set; }

    [Name("Feedback")]
    public FeebackCrowdfundingCartItemReq Feedback { get; set; }
    
    [Name("Quantity")]
    public int Quantity { get; set; }
}
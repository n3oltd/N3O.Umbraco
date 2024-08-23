using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserAllocationReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Goal Id")]
    public Guid? GoalId { get; set; }
    
    [Name("Feedback Custom Fields")]
    public FeedbackNewCustomFieldsReq FeedbackNewCustomFields { get; set; }
}
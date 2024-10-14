using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsReq {
    [Name("Items")]
    public IEnumerable<FundraiserGoalReq> Items { get; set; }
}
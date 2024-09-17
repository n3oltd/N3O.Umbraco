using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsReq {
    public IEnumerable<FundraiserGoalReq> Items { get; set; }
}
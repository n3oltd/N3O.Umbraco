using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class UpdateFundraiserGoalsReq {
    public IEnumerable<UpdateFundraiserGoalReq> Goals { get; set; }
}
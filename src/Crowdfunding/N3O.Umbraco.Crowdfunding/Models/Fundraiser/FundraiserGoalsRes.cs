using N3O.Umbraco.Financial;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class FundraiserGoalsRes {
    public CurrencyRes Currency { get; set; }
    public IEnumerable<AvailableGoalRes> Available { get; set; }
    public IEnumerable<SelectedGoalRes> Selected { get; set; }
}
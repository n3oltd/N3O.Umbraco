using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalOptionFundDimensionRes {
    public FundDimensionValueRes Default { get; set; }
    public IEnumerable<FundDimensionValueRes> AllowedOptions { get; set; }
}
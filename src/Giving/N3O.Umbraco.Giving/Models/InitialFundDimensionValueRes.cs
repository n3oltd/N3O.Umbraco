using N3O.Giving.Models;

namespace N3O.Umbraco.Giving.Models;

public class InitialFundDimensionValueRes {
    public FundDimensionValueRes Fixed { get; set; }
    public FundDimensionValueRes Default { get; set; }
    public FundDimensionValueRes Suggested { get; set; }
}

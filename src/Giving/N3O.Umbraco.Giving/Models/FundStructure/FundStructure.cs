using N3O.Umbraco;

namespace N3O.Giving.Models;

public class FundStructure : Value {
    public FundStructure(FundDimension1 dimension1,
                         FundDimension2 dimension2,
                         FundDimension3 dimension3,
                         FundDimension4 dimension4) {
        Dimension1 = dimension1;
        Dimension2 = dimension2;
        Dimension3 = dimension3;
        Dimension4 = dimension4;
    }

    public FundDimension1 Dimension1 { get; }
    public FundDimension2 Dimension2 { get; }
    public FundDimension3 Dimension3 { get; }
    public FundDimension4 Dimension4 { get; }
}

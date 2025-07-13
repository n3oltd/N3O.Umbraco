using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFundDimensionValues : IFundDimensionValues {
    public FundDimension1Value Dimension1 { get; set; }
    public FundDimension2Value Dimension2 { get; set; }
    public FundDimension3Value Dimension3 { get; set; }
    public FundDimension4Value Dimension4 { get; set; }
    
    [JsonIgnore]
    public IEnumerable<IFundDimensionValue> Dimensions {
        get {
            yield return Dimension1;
            yield return Dimension2;
            yield return Dimension3;
            yield return Dimension4;
        }
    }
}
using N3O.Umbraco.Giving.Allocations.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFundDimensionOptions : IFundDimensionOptions {
    public IEnumerable<FundDimension1Value> Dimension1 { get; set; }
    public IEnumerable<FundDimension2Value> Dimension2 { get; set; }
    public IEnumerable<FundDimension3Value> Dimension3 { get; set; }
    public IEnumerable<FundDimension4Value> Dimension4 { get; set; }
    
    [JsonIgnore]
    public IEnumerable<IEnumerable<IFundDimensionValue>> Dimensions {
        get {
            yield return Dimension1;
            yield return Dimension2;
            yield return Dimension3;
            yield return Dimension4;
        }
    }
}
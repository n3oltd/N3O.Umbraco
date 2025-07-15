using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedFundDimensionView {
    public FundDimensionSelector Selector { get; set; }
    public PublishedFundDimensionToggleOptions Toggle { get; set; }
}
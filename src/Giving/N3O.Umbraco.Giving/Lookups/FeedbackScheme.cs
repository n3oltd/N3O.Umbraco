using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Lookups;

public class FeedbackScheme :
    LookupContent<FeedbackScheme>,
    IFundDimensionsOptions {
    public IEnumerable<FundDimension1Value> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
    public IEnumerable<FundDimension2Value> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
    public IEnumerable<FundDimension3Value> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
    public IEnumerable<FundDimension4Value> Dimension4Options => GetPickedAs(x => x.Dimension4Options);

    public IEnumerable<FeedbackComponent> Components => Content()
                                                        .Children.As<FeedbackComponent>();
}
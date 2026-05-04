using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniItem : ContentOrPublishedLookup, IHoldFundDimensionOptions {
    public QurbaniItem(string id, string name, Guid? contentId, FundDimensionValues fundDimensionValues, Price price)
        : base(id, name, contentId) {
        FundDimensionValues = fundDimensionValues;
        Price = price;
    }

    public FundDimensionValues FundDimensionValues { get; }
    public Price Price { get; }

    public IFundDimensionOptions FundDimensionOptions => FundDimensionValues.ToFundDimensionOptions();
}

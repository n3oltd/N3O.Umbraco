using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Content;

public class UpsellOfferContent : UmbracoContent<UpsellOfferContent> {
    public bool AllowMultiple => GetValue(x => x.AllowMultiple);
    public string Description => GetValue(x => x.Description);
    public DonationItem DonationItem => GetAs(x => x.DonationItem);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public GivingType GivingType => GetValue(x => x.GivingType);
    public decimal? FixedAmount => GetValue(x => x.FixedAmount);
    public IEnumerable<GivingType> OfferedFor => GetValue(x => x.OfferedFor);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);

    public FundDimensionValues FundDimensions => new(Dimension1,
                                                     Dimension2,
                                                     Dimension3,
                                                     Dimension4);
}
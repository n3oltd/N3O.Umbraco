using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class UpsellOfferContent : UmbracoContent<UpsellOfferContent> {
    public bool AllowMultiple => GetValue(x => x.AllowMultiple);
    public string Description => GetValue(x => x.Description);
    public GivingType GivingType => GetValue(x => x.GivingType);
    public decimal? FixedAmount => GetValue(x => x.FixedAmount);
    public IEnumerable<GivingType> OfferedFor => GetValue(x => x.OfferedFor);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);
    
    public FundDimension1Value GetDimension1(ILookups lookups) => GetLookup<FundDimension1Value>(lookups, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension1);
    public FundDimension2Value GetDimension2(ILookups lookups) => GetLookup<FundDimension2Value>(lookups, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension2);
    public FundDimension3Value GetDimension3(ILookups lookups) => GetLookup<FundDimension3Value>(lookups, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension3);
    public FundDimension4Value GetDimension4(ILookups lookups) => GetLookup<FundDimension4Value>(lookups, AllocationsConstants.Aliases.UpsellOffer.Properties.Dimension4);

    public FundDimensionValues GetFundDimensions(ILookups lookups) => new(GetDimension1(lookups),
                                                                          GetDimension2(lookups),
                                                                          GetDimension3(lookups),
                                                                          GetDimension4(lookups));

    public DonationItem GetDonationItem(ILookups lookups) {
        return GetLookup<DonationItem>(lookups, AllocationsConstants.Aliases.UpsellOffer.Properties.DonationItem);
    }
}
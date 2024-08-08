using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfundingPageAllocationElement : UmbracoElement<CrowdfundingPageAllocationElement>, IFundDimensionValues {
    public string Title => GetValue(x => x.Title);
    public decimal Amount => GetValue(x => x.Amount);
    public AllocationType Type => GetPickedAs(x => x.Type);
    public FundDimension1Value Dimension1 => GetAs(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetAs(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetAs(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetAs(x => x.Dimension4);
    public IEnumerable<PriceHandleElement> PriceHandles => GetNestedAs(x => x.PriceHandles);

    public DonationItem DonationItem => GetPickedAs(x => x.DonationItem);
    public SponsorshipScheme SponsorshipScheme => GetPickedAs(x => x.SponsorshipScheme);
    public FeedbackScheme FeedbackScheme => GetPickedAs(x => x.FeedbackScheme);
    
    public IFundDimensionsOptions GetFundDimensionOptions() {
        return (IFundDimensionsOptions) DonationItem ??
               (IFundDimensionsOptions) SponsorshipScheme ??
               (IFundDimensionsOptions) FeedbackScheme;
    }
}

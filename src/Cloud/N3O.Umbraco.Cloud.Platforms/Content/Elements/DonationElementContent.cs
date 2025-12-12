using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonationElement)]
public class DonationElementContent<T> : UmbracoContent<T> where T : DonationElementContent<T> {
    public OfferingContent Offering => GetAs(x => x.Offering);
    public FundDimension1Value Dimension1 => GetValue(x => x.Dimension1);
    public FundDimension2Value Dimension2 => GetValue(x => x.Dimension2);
    public FundDimension3Value Dimension3 => GetValue(x => x.Dimension3);
    public FundDimension4Value Dimension4 => GetValue(x => x.Dimension4);
    
    public IFundDimensionValues GetFixedFundDimensionValues(OfferingContent offering) {
        var offeringFixedFundDimensionValues = offering.GetFixedFundDimensionValues();
        
        var dimension1 = Dimension1 ?? offeringFixedFundDimensionValues.Dimension1;
        var dimension2 = Dimension2 ?? offeringFixedFundDimensionValues.Dimension2;
        var dimension3 = Dimension3 ?? offeringFixedFundDimensionValues.Dimension3;
        var dimension4 = Dimension4 ?? offeringFixedFundDimensionValues.Dimension4;
        
        return new FundDimensionValues(dimension1, dimension2, dimension3, dimension4);
    }
}
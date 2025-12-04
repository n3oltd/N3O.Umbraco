using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class OfferingContentExtensions {
    public static OfferingFundDimensionsOptionsReq ToOfferingFundDimensionReq(this OfferingContent offeringContent) {
        var fundDimensionOptions = offeringContent.GetFundDimensionOptions();
        
        var fundDimensions = new OfferingFundDimensionsOptionsReq();
        fundDimensions.Dimension1 = ToPublishedOfferingFundDimension(offeringContent.Dimension1,
                                                                     fundDimensionOptions.Dimension1);
        
        fundDimensions.Dimension2 = ToPublishedOfferingFundDimension(offeringContent.Dimension2,
                                                                     fundDimensionOptions.Dimension2);
        
        fundDimensions.Dimension3 = ToPublishedOfferingFundDimension(offeringContent.Dimension3,
                                                                     fundDimensionOptions.Dimension3);
        
        fundDimensions.Dimension4 = ToPublishedOfferingFundDimension(offeringContent.Dimension4,
                                                                     fundDimensionOptions.Dimension4);

        return fundDimensions;
    }
    
    public static OfferingFundDimensionOptionsReq ToPublishedOfferingFundDimension(this IFundDimensionValue fundDimensionValue,
                                                                                   IEnumerable<IFundDimensionValue> fundDimensionOptions) {
        if (fundDimensionOptions.None()) {
            return null;
        }
        
        var offeringFundDimension = new OfferingFundDimensionOptionsReq();
        offeringFundDimension.Options = fundDimensionOptions.Select(x => x.Name).OrEmpty().ToList();

        if (fundDimensionValue != null) {
            offeringFundDimension.Fixed = fundDimensionValue.Name;
        } else if (fundDimensionOptions.IsSingle()) {
            offeringFundDimension.Fixed = fundDimensionOptions.Single().Name;
        } else {
            offeringFundDimension.Suggested = fundDimensionOptions.FirstOrDefault(x => x.IsUnrestricted)?.Name;
        }

        return offeringFundDimension;
    }
}
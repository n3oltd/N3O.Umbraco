using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class DesignationContentExtensions {
    public static PublishedDesignationFundDimensions ToPublishedDesignationFundDimensions(this DesignationContent designationContent,
                                                                                          ILookups lookups) {
        var fundDimensionOptions = designationContent.GetFundDimensionOptions(lookups);
        
        var fundDimensions = new PublishedDesignationFundDimensions();
        fundDimensions.Dimension1 = ToPublishedDesignationFundDimension(designationContent.GetDimension1(lookups),
                                                                        fundDimensionOptions.Dimension1);
        
        fundDimensions.Dimension2 = ToPublishedDesignationFundDimension(designationContent.GetDimension2(lookups),
                                                                        fundDimensionOptions.Dimension2);
        
        fundDimensions.Dimension3 = ToPublishedDesignationFundDimension(designationContent.GetDimension3(lookups),
                                                                        fundDimensionOptions.Dimension3);
        
        fundDimensions.Dimension4 = ToPublishedDesignationFundDimension(designationContent.GetDimension4(lookups),
                                                                        fundDimensionOptions.Dimension4);

        return fundDimensions;
    }
    
    private static PublishedDesignationFundDimension ToPublishedDesignationFundDimension(IFundDimensionValue fundDimensionValue,
                                                                                         IEnumerable<IFundDimensionValue> fundDimensionOptions) {
        if (fundDimensionOptions.None()) {
            return null;
        }
        
        var designationFundDimension = new PublishedDesignationFundDimension();
        designationFundDimension.Options = fundDimensionOptions.Select(x => x.Name).ToList();

        if (fundDimensionValue != null) {
            designationFundDimension.Fixed = fundDimensionValue.Name;
        } else if (fundDimensionOptions.IsSingle()) {
            designationFundDimension.Fixed = fundDimensionOptions.Single().Name;
        } else {
            designationFundDimension.Suggested = fundDimensionOptions.FirstOrDefault(x => x.IsUnrestricted)?.Name;
        }

        return designationFundDimension;
    }
}
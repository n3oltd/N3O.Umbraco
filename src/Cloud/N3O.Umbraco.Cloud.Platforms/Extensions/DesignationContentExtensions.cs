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
        var fundDimensionValues = designationContent.GetFundDimensionValues(lookups);
        var fundDimensionOptions = designationContent.GetFundDimensionOptions(lookups);
        
        var fundDimensions = new PublishedDesignationFundDimensions();
        fundDimensions.Dimension1 = ToPublishedDesignationFundDimension(fundDimensionValues.Dimension1,
                                                                        fundDimensionOptions.Dimension1);
        
        fundDimensions.Dimension2 = ToPublishedDesignationFundDimension(fundDimensionValues.Dimension2,
                                                                        fundDimensionOptions.Dimension2);
        
        fundDimensions.Dimension3 = ToPublishedDesignationFundDimension(fundDimensionValues.Dimension3,
                                                                        fundDimensionOptions.Dimension3);
        
        fundDimensions.Dimension4 = ToPublishedDesignationFundDimension(fundDimensionValues.Dimension4,
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
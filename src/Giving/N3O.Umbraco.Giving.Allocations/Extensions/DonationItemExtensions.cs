using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions {
    public static class DonationItemExtensions {
        public static IEnumerable<DonationType> GetDonationTypes(DonationItem donationItem) {
            if (donationItem.AllowSingleDonations) {
                yield return DonationTypes.Single;
            }
        
            if (donationItem.AllowRegularDonations) {
                yield return DonationTypes.Regular;
            }
        }
    
        public static FundDimension1Option DefaultFundDimension1(this IHoldFundDimensionOptions holdFundDimensionOptions) {
            return DefaultFundDimension(holdFundDimensionOptions.Dimension1Options);
        }

        public static FundDimension2Option DefaultFundDimension2(this IHoldFundDimensionOptions holdFundDimensionOptions) {
            return DefaultFundDimension(holdFundDimensionOptions.Dimension2Options);
        }
    
        public static FundDimension3Option DefaultFundDimension3(this IHoldFundDimensionOptions holdFundDimensionOptions) {
            return DefaultFundDimension(holdFundDimensionOptions.Dimension3Options);
        }
    
        public static FundDimension4Option DefaultFundDimension4(this IHoldFundDimensionOptions holdFundDimensionOptions) {
            return DefaultFundDimension(holdFundDimensionOptions.Dimension4Options);
        }
    
        private static T DefaultFundDimension<T>(IEnumerable<T> fundDimensionValues) where T : FundDimensionOption {
            return fundDimensionValues.OrEmpty().FirstOrDefault(x => x.IsUnrestricted);
        }
    }
}

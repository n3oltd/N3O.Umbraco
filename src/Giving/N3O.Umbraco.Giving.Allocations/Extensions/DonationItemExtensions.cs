using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Extensions {
    public static class DonationItemExtensions {
        public static IEnumerable<DonationType> GetDonationTypes(this DonationItem donationItem) {
            if (donationItem.AllowSingleDonations) {
                yield return DonationTypes.Single;
            }
        
            if (donationItem.AllowRegularDonations) {
                yield return DonationTypes.Regular;
            }
        }
    }
}

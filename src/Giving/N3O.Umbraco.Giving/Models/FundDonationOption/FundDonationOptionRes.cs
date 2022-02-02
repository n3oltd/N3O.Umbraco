using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models {
    public class FundDonationOptionRes {
        public DonationItem DonationItem { get; set; }
        public IEnumerable<PriceHandleRes> DonationPriceHandles { get; set; }
        public IEnumerable<PriceHandleRes> RegularGivingPriceHandles { get; set; }
    }
}
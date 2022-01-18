using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class FundDonationOptionRes {
        public DonationItem DonationItem { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension1 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension2 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension3 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension4 { get; set; }
        public bool ShowQuantity { get; set; }
        public bool HideSingle { get; set; }
        public IEnumerable<PriceHandleRes> SinglePriceHandles { get; set; }
        public bool HideRegular { get; set; }
        public IEnumerable<PriceHandleRes> RegularPriceHandles { get; set; }
    }
}
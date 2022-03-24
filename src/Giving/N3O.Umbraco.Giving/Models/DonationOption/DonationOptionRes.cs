using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Giving.Models {
    public class DonationOptionRes {
        public AllocationType Type { get; set; }
        public FixedOrDefaultFundDimensionValueRes Dimension1 { get; set; }
        public FixedOrDefaultFundDimensionValueRes Dimension2 { get; set; }
        public FixedOrDefaultFundDimensionValueRes Dimension3 { get; set; }
        public FixedOrDefaultFundDimensionValueRes Dimension4 { get; set; }
        public bool HideQuantity { get; set; }
        public bool HideDonation { get; set; }
        public bool HideRegularGiving { get; set; }
        public bool RegularGivingIsDefault { get; set; }
        public FundDonationOptionRes Fund { get; set; }
        public SponsorshipDonationOptionRes Sponsorship { get; set; }
    }
}
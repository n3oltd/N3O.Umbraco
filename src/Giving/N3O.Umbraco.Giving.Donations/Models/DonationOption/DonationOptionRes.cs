using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class DonationOptionRes {
        public AllocationType Type { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension1 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension2 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension3 { get; set; }
        public FixedOrDefaultFundDimensionOptionRes Dimension4 { get; set; }
        public bool HideQuantity { get; set; }
        public bool HideDonation { get; set; }
        public bool HideRegularGiving { get; set; }
        public FundDonationOptionRes Fund { get; set; }
        public SponsorshipDonationOptionRes Sponsorship { get; set; }
    }
}
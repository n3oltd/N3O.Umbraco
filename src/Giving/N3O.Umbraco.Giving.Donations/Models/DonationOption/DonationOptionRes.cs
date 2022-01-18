using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class DonationOptionRes {
        public AllocationType Type { get; set; }
        public FundDonationOptionRes Fund { get; set; }
        public SponsorshipDonationOptionRes Sponsorship { get; set; }
    }
}
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class DonationFormRes {
        public DonationItem Item { get; set; }
        public DonationOptionsRes Regular { get; set; }
        public DonationOptionsRes Single { get; set; }
    }
}
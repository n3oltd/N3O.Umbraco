using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Models {
    public class FundAllocation : Value, IFundAllocation {
        [JsonConstructor]
        public FundAllocation(DonationItem donationItem) {
            DonationItem = donationItem;
        }

        public FundAllocation(IFundAllocation fund) : this(fund.DonationItem) { }

        public DonationItem DonationItem { get; }
    
        public string Summary => DonationItem?.Name;
    }
}
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class DonationItemRes : NamedLookupRes {
        public bool AllowSingleDonations { get; set; }
        public bool AllowRegularDonations { get; set; }
        public bool Free { get; set; }
        public MoneyRes Price { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension1Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension2Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension3Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension4Options { get; set; }
    }
}
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups {
    public class DonationItem : LookupContent<DonationItem>, IHoldPrice, IHoldFundDimensionOptions {
        public bool AllowSingleDonations => GetValue(x => x.AllowSingleDonations);
        public bool AllowRegularDonations => GetValue(x => x.AllowRegularDonations);
        public bool Free => GetValue(x => x.Free);
        public decimal Price => GetValue(x => x.Price);
        public IEnumerable<FundDimension1Option> Dimension1Options => GetPickedAs(x => x.Dimension1Options);
        public IEnumerable<FundDimension2Option> Dimension2Options => GetPickedAs(x => x.Dimension2Options);
        public IEnumerable<FundDimension3Option> Dimension3Options => GetPickedAs(x => x.Dimension3Options);
        public IEnumerable<FundDimension4Option> Dimension4Options => GetPickedAs(x => x.Dimension4Options);
    }
}

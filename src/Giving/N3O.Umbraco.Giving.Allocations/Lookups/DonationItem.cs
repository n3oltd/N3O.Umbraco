using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class DonationItem : LookupContent, IHoldPrice, IHoldFundDimensionOptions {
    public bool AllowSingleDonations => GetValue<DonationItem, bool>(x => x.AllowSingleDonations);
    public bool AllowRegularDonations => GetValue<DonationItem, bool>(x => x.AllowRegularDonations);
    public bool Free => GetValue<DonationItem, bool>(x => x.Free);
    public decimal Price => GetValue<DonationItem, decimal?>(x => x.Price) ?? 0;
    public IEnumerable<FundDimension1Option> Dimension1Options => GetValue<DonationItem, IEnumerable<FundDimension1Option>>(x => x.Dimension1Options);
    public IEnumerable<FundDimension2Option> Dimension2Options => GetValue<DonationItem, IEnumerable<FundDimension2Option>>(x => x.Dimension2Options);
    public IEnumerable<FundDimension3Option> Dimension3Options => GetValue<DonationItem, IEnumerable<FundDimension3Option>>(x => x.Dimension3Options);
    public IEnumerable<FundDimension4Option> Dimension4Options => GetValue<DonationItem, IEnumerable<FundDimension4Option>>(x => x.Dimension4Options);
}

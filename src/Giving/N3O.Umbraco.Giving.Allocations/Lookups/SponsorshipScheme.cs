using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class SponsorshipScheme : LookupContent, IHoldPrice, IHoldFundDimensionOptions {
    public bool AllowSingleDonations => GetValue<SponsorshipScheme, bool>(x => x.AllowSingleDonations);
    public bool AllowRegularDonations => GetValue<SponsorshipScheme, bool>(x => x.AllowRegularDonations);
    public decimal Price => GetValue<SponsorshipScheme, decimal?>(x => x.Price) ?? 0;
    public IEnumerable<FundDimension1Option> Dimension1Options => GetValue<SponsorshipScheme, IEnumerable<FundDimension1Option>>(x => x.Dimension1Options);
    public IEnumerable<FundDimension2Option> Dimension2Options => GetValue<SponsorshipScheme, IEnumerable<FundDimension2Option>>(x => x.Dimension2Options);
    public IEnumerable<FundDimension3Option> Dimension3Options => GetValue<SponsorshipScheme, IEnumerable<FundDimension3Option>>(x => x.Dimension3Options);
    public IEnumerable<FundDimension4Option> Dimension4Options => GetValue<SponsorshipScheme, IEnumerable<FundDimension4Option>>(x => x.Dimension4Options);
}

using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Pricing.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class DonationItemRes : NamedLookupRes {
        public IEnumerable<GivingType> AllowedGivingTypes { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension1Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension2Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension3Options { get; set; }
        public IEnumerable<FundDimensionOptionRes> Dimension4Options { get; set; }
        public PricingRes Pricing { get; set; }
    }
}
using N3O.Umbraco.Extensions;
using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Sponsorships.Lookups {
    public class SponsorshipScheme :
        LookupContent<SponsorshipScheme>, IHoldFundDimensionOptions, IHoldAllowedGivingTypes {
        public IEnumerable<GivingType> AllowedGivingTypes => GetValue(x => x.AllowedGivingTypes);
        public IEnumerable<FundDimension1Option> Dimension1Options => GetValue(x => x.Dimension1Options);
        public IEnumerable<FundDimension2Option> Dimension2Options => GetValue(x => x.Dimension2Options);
        public IEnumerable<FundDimension3Option> Dimension3Options => GetValue(x => x.Dimension3Options);
        public IEnumerable<FundDimension4Option> Dimension4Options => GetValue(x => x.Dimension4Options);
        public IEnumerable<SponsorshipSchemeComponent> Components => Content.Children.As<SponsorshipSchemeComponent>();
    }
}

using N3O.Giving.Models;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models {
    public class SponsorshipSchemeRes : NamedLookupRes {
        public IEnumerable<GivingType> AllowedGivingTypes { get; set; }
        public IEnumerable<SponsorshipDuration> AllowedDurations { get; set; }
        public IEnumerable<FundDimensionValueRes> Dimension1Options { get; set; }
        public IEnumerable<FundDimensionValueRes> Dimension2Options { get; set; }
        public IEnumerable<FundDimensionValueRes> Dimension3Options { get; set; }
        public IEnumerable<FundDimensionValueRes> Dimension4Options { get; set; }
        public IEnumerable<SponsorshipComponentRes> Components { get; set; }
    }
}
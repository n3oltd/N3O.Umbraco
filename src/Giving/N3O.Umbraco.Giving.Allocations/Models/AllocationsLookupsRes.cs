using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class AllocationsLookupsRes : LookupsRes {
        [FromLookupType(AllocationsLookupTypes.AllocationTypes)]
        public IEnumerable<NamedLookupRes> AllocationTypes { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.DonationItems)]
        public IEnumerable<DonationItemRes> DonationItems { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.DonationTypes)]
        public IEnumerable<NamedLookupRes> DonationTypes { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension1Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension1Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension2Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension2Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension3Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension3Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension4Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension4Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.SponsorshipSchemes)]
        public IEnumerable<NamedLookupRes> SponsorshipSchemes { get; set; }
    }
}
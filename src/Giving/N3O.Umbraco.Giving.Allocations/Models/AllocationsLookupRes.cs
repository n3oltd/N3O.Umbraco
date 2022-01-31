using N3O.Umbraco.FundDimensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class AllocationsLookupRes : LookupsRes {
        [FromLookupType(AllocationsLookupTypes.AllocationTypes)]
        public IEnumerable<NamedLookupRes> AllocationTypes { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.DonationItems)]
        public IEnumerable<DonationItemRes> DonationItems { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.GivingTypes)]
        public IEnumerable<NamedLookupRes> GivingTypes { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension1Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension1Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension2Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension2Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension3Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension3Options { get; set; }
        
        [FromLookupType(AllocationsLookupTypes.FundDimension4Options)]
        public IEnumerable<FundDimensionOptionRes> FundDimension4Options { get; set; }
    }
}
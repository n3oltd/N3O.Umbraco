using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public class SponsorshipAllocationReq : ISponsorshipAllocation {
        [Name("Scheme")]
        public SponsorshipScheme Scheme { get; set; }
    }
}
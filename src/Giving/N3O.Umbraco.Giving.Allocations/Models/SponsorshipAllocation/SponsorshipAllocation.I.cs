using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public interface ISponsorshipAllocation {
        SponsorshipScheme Scheme { get; }
    }
}
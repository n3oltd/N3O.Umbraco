using N3O.Umbraco.Giving.Sponsorships.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Models {
    public interface ISponsorshipAllocation {
        SponsorshipScheme Scheme { get; }
    }
}
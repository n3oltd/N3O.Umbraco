using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Extensions;

public static class SponsorshipComponentExtensions {
    public static SponsorshipScheme GetSponsorshipScheme(this SponsorshipComponent sponsorshipComponent,
                                                         ILookups lookups) {
        return lookups.FindById<SponsorshipScheme>(sponsorshipComponent.SchemeId);
    }
}
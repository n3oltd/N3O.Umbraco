using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Sponsorships.Lookups {
    public class SponsorshipsLookupTypes : ILookupTypesSet {
        [LookupInfo(typeof(SponsorshipDuration))]
        public const string SponsorshipDurations = "sponsorshipDurations";
            
        [LookupInfo(typeof(SponsorshipScheme))]
        public const string SponsorshipSchemes = "sponsorshipSchemes";
    }
}
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Sponsorships.Lookups {
    public class SponsorshipDurationDataSource : LookupsDataSource<SponsorshipDuration> {
        public SponsorshipDurationDataSource(ILookups lookups) : base(lookups) { }
        
        public override string Name => "Sponsorship Durations";
        public override string Description => "Data source for sponsorship durations";
        public override string Icon => "icon-timer";

        protected override string GetIcon(SponsorshipDuration sponsorshipDuration) => "icon-hourglass";
    }
}
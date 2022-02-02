using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups {
    public class SponsorshipDuration : NamedLookup {
        public SponsorshipDuration(string id, string name, int months) : base(id, name) {
            Months = months;
        }

        public int Months { get; }
    }

    public class SponsorshipDurations : StaticLookupsCollection<SponsorshipDuration> {
        public static readonly SponsorshipDuration SixMonths = new SponsorshipDuration("_6", "6 Months", 6);
        public static readonly SponsorshipDuration TwelveMonths = new SponsorshipDuration("_12", "12 Months", 12);
        public static readonly SponsorshipDuration EighteenMonths = new SponsorshipDuration("_18", "18 Months", 18);
        public static readonly SponsorshipDuration TwentyFourMonths = new SponsorshipDuration("_24", "24 Months", 24);
    }
}
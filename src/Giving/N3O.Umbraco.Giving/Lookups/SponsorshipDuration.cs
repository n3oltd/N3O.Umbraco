using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class SponsorshipDuration : NamedLookup {
    public SponsorshipDuration(string id, string name, int months) : base(id, name) {
        Months = months;
    }

    public int Months { get; }
}

public class SponsorshipDurations : StaticLookupsCollection<SponsorshipDuration> {
    public static readonly SponsorshipDuration SixMonths = new("_6", "6 Months", 6);
    public static readonly SponsorshipDuration TwelveMonths = new("_12", "12 Months", 12);
    public static readonly SponsorshipDuration EighteenMonths = new("_18", "18 Months", 18);
    public static readonly SponsorshipDuration TwentyFourMonths = new("_24", "24 Months", 24);
    public static readonly SponsorshipDuration ThirtySixMonths = new("_36", "36 Months", 36);
    public static readonly SponsorshipDuration FourtyEightMonths = new("_48", "48 Months", 48);
    public static readonly SponsorshipDuration SixtyMonths = new("_60", "60 Months", 60);
}

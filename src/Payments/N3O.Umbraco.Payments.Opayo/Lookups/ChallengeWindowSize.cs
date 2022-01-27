using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Payments.Opayo.Lookups {
    public class ChallengeWindowSize : NamedLookup {
        public ChallengeWindowSize(string id, string name) : base(id, name) { }
    }

    public class ChallengeWindowSizes : StaticLookupsCollection<ChallengeWindowSize> {
        public static readonly ChallengeWindowSize Small = new("small", "Small");
        public static readonly ChallengeWindowSize Medium = new("medium", "Medium");
        public static readonly ChallengeWindowSize Large = new("large", "Large");
        public static readonly ChallengeWindowSize ExtraLarge = new("extraLarge", "Extra Large");
        public static readonly ChallengeWindowSize FullScreen = new("fullScreen", "Full Screen");
    }
}
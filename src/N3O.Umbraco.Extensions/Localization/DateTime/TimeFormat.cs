using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Localization {
    public class TimeFormat : NamedLookup {
        public TimeFormat(string id, string name, bool hasMeridiem, string cultureCode) : base(id, name) {
            HasMeridiem = hasMeridiem;
            CultureCode = cultureCode;
        }

        public bool HasMeridiem { get; }
        public string CultureCode { get; }
    }

    public class TimeFormats : StaticLookupsCollection<TimeFormat> {
        public static readonly TimeFormat _12 = new TimeFormat("12", "12 Hour Format", true, "en-US");
        public static readonly TimeFormat _24 = new TimeFormat("24", "24 Hour Format", false, "en-GB");
    }
}
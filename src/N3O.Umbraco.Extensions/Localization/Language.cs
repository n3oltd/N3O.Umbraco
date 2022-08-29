using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Localization;

public class Language : NamedLookup {
    public Language(string id, string name) : base(id, name) { }
}

public class Languages : StaticLookupsCollection<Language> {
    public static readonly Language English = new("en", "English");
    public static readonly Language French = new("fr", "French");
    public static readonly Language Spanish = new("es", "Spanish");

    public static readonly Language LocalizationTest = new("xx", "Localization Test");
}

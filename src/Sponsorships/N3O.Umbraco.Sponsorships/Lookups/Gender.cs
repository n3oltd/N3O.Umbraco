using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Sponsorships.Lookups {
    public class Gender : NamedLookup {
        public Gender(string id, string name) : base(id, name) { }
    }

    public class Genders : StaticLookupsCollection<Gender> {
        public static readonly Gender Female = new Gender("female", "Female");
        public static readonly Gender Male = new Gender("male", "Male");
    }
}

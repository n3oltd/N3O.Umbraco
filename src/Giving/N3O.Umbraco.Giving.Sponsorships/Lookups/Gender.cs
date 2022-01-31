using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Sponsorships.Lookups {
    public class Gender : NamedLookup {
        public Gender(string id, string name) : base(id, name) { }
    }

    public class Genders : StaticLookupsCollection<Gender> {
        public static readonly Gender Female = new("female", "Female");
        public static readonly Gender Male = new("male", "Male");
    }
}

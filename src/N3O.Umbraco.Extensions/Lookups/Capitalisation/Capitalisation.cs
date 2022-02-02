namespace N3O.Umbraco.Lookups {
    public class Capitalisation : NamedLookup {
        public Capitalisation(string id, string name) : base(id, name) { }
    }

    public class Capitalisations : StaticLookupsCollection<Capitalisation> {
        public static readonly Capitalisation Lower = new("lower", "Lower");
        public static readonly Capitalisation Title = new("title", "Title");
        public static readonly Capitalisation Upper = new("upper", "Upper");
    }
}
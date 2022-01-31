using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups {
    public class GivingType : NamedLookup {
        public GivingType(string id, string name, string icon, int order) : base(id, name) {
            Icon = icon;
            Order = order;
        }

        public string Icon { get; }
        public int Order { get; }
    }

    public class GivingTypes : StaticLookupsCollection<GivingType> {
        public static readonly GivingType Donation = new("donation", "Donation", "icon-coin-dollar", 0);
        public static readonly GivingType RegularGiving = new("regularGiving", "Regular Giving", "icon-coins-dollar", 1);
    }
}

using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class OrganizationType : NamedLookup {
    public OrganizationType(string id, string name) : base(id, name) { }
}

public class OrganizationTypes : StaticLookupsCollection<OrganizationType> {
    public static readonly OrganizationType Business = new("business", "Business");
}
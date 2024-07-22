using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Authentication.Auth0.Lookups;

public class ClientType : NamedLookup {
    public ClientType(string id, string name) : base(id, name) { }
}

public class ClientTypes : StaticLookupsCollection<ClientType> {
    public static readonly ClientType BackOffice = new("backOffice", "BackOffice");
    public static readonly ClientType Members = new("members", "Members");
}
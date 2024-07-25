using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Authentication.Auth0.Lookups;

public class ClientType : Lookup {
    public ClientType(string id) : base(id) { }
}

public class ClientTypes : StaticLookupsCollection<ClientType> {
    public static readonly ClientType BackOffice = new("backOffice");
    public static readonly ClientType Members = new("members");
}
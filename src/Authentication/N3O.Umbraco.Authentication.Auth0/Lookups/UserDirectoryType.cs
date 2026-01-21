using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Authentication.Auth0.Lookups;

public class UserDirectoryType : Lookup {
    public UserDirectoryType(string id) : base(id) { }
}

public class UserDirectoryTypes : StaticLookupsCollection<UserDirectoryType> {
    public static readonly UserDirectoryType BackOffice = new("backOffice");
    public static readonly UserDirectoryType Members = new("members");
}
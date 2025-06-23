using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Authentication.Auth0.Lookups;

public class UmbracoAuthType : Lookup {
    public UmbracoAuthType(string id) : base(id) { }
}

public class UmbracoAuthTypes : StaticLookupsCollection<UmbracoAuthType> {
    public static readonly UmbracoAuthType Member = new("member");
    public static readonly UmbracoAuthType User = new("user");
}
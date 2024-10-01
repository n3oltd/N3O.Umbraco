using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Authentication.Auth0.NamedParameters;

public class UserDirectoryId : NamedParameter<string> {
    public override string Name => "userDirectoryId";
}
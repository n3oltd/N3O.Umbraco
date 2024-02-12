namespace N3O.Umbraco.Authentication.Auth0.Options;

public class Auth0BackOfficeAuthenticationOptions : Auth0Credentials {
    public bool AutoCreateDirectoryUser { get; set; }
    public string ConnectionName { get; set; }
}
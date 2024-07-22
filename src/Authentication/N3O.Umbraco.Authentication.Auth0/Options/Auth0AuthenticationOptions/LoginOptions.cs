namespace N3O.Umbraco.Authentication.Auth0.Options;

public class LoginOptions : Auth0Application {
    public string Authority { get; set; }
    public bool AutoCreateDirectoryUser { get; set; }
    public string ConnectionName { get; set; }
}
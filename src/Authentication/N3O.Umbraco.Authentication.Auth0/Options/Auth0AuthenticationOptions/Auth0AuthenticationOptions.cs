namespace N3O.Umbraco.Authentication.Auth0.Options;

public class Auth0AuthenticationOptions {
    public string ApiBaseUrl { get; set; }
    public M2MClientOptions M2MClient { get; set; }
    public ManagementClientOptions ManagementClient { get; set; }
}
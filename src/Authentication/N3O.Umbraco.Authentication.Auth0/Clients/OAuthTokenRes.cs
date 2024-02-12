namespace N3O.Umbraco.Authentication.Auth0.Clients;

public class OAuthTokenRes {
    public string AccessToken { get; set; }
    public long ExpiresIn { get; set; }
}

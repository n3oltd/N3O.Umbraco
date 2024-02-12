namespace N3O.Umbraco.Authentication.Auth0.Clients;

public class OAuthTokenReq {
    public string GrantType { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string Audience { get; set; }
}

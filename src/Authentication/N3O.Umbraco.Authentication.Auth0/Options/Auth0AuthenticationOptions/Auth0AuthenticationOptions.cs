namespace N3O.Umbraco.Authentication.Auth0.Options;

public class Auth0AuthenticationOptions {
    public LoginOptions Login { get; set; }
    public M2MOptions M2M { get; set; }
    public ManagementOptions Management { get; set; }
}
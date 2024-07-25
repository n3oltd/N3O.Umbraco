namespace N3O.Umbraco.Authentication.Auth0.Options;

public class AuthenticationOptions {
    public Auth0BackOfficeAuthenticationOptions BackOffice { get; set; }
    public Auth0MemberAuthenticationOptions Members { get; set; }
}
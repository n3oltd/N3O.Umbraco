namespace N3O.Umbraco.Authentication.Auth0.Options {
    public abstract class Auth0AuthenticationOptions {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
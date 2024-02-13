namespace N3O.Umbraco.Authentication.Auth0;

public static class Auth0AuthenticationConstants {
    public static class Configuration {
        public const string Section = "Auth0";
        
        public static class Keys {
            public static readonly string Authority = nameof(Authority);
            public static readonly string ClientId = nameof(ClientId);
            public static readonly string ClientSecret = nameof(ClientSecret);
        }
    }
}

namespace N3O.Umbraco.Security {
    public class AuthorizedResult {
        public AuthorizedResult(bool authorized, UnauthorizedReason unauthorizedReason) {
            Authorized = authorized;
            UnauthorizedReason = unauthorizedReason;
        }

        public bool Authorized { get; }
        public UnauthorizedReason UnauthorizedReason { get; }

        public static AuthorizedResult IsAuthorized() {
            return new AuthorizedResult(true, null);
        }

        public static AuthorizedResult IsUnauthorized(UnauthorizedReason reason) {
            return new AuthorizedResult(false, reason);
        }
    }
}
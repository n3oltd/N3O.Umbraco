using Auth0.ManagementApi;
using System.Collections.Generic;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0User {
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }
    public IEnumerable<UserIdentitySchema> Identities { get; set; }
}

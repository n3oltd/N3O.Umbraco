using Auth0.ManagementApi;
using NodaTime;
using System.Collections.Generic;

namespace N3O.Umbraco.Authentication.Auth0;

public class Auth0User {
    public string Id { get; set; }
    public string Email { get; set; }
    public string Picture { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Blocked { get; set; }
    public bool EmailVerified { get; set; }
    public Instant? LastLogin { get; set; }
    public string LastIpAddress { get; set; }
    public bool IsFederated { get; set; }
    public IEnumerable<UserIdentitySchema> Identities { get; set; }
}

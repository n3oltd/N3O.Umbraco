using Auth0.ManagementApi;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Authentication.Auth0;

internal static class Auth0UserMapping {
    public static Auth0User ToAuth0User(this UserResponseSchema user) {
        if (!user.HasValue()) {
            return null;
        }

        return new Auth0User {
            UserId = user.UserId,
            Email = user.Email,
            Picture = user.Picture,
            Identities = user.Identities
        };
    }

    public static Auth0User ToAuth0User(this CreateUserResponseContent user) {
        if (!user.HasValue()) {
            return null;
        }

        return new Auth0User {
            UserId = user.UserId,
            Email = user.Email,
            Picture = user.Picture,
            Identities = user.Identities
        };
    }
}

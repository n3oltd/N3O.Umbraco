using N3O.Umbraco.Payments.Bambora.Clients;

namespace N3O.Umbraco.Payments.Bambora.Extensions;

public static class ApiProfileResExtensions {
    public static bool IsSuccessful(this ApiProfileRes profile) {
        return HasCode(profile, 1);
    }
    
    private static bool HasCode(ApiProfileRes profile, int code) {
        return profile.Code == code;
    }
}

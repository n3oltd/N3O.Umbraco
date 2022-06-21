using N3O.Umbraco.Lookups;
using System.Net;

namespace N3O.Umbraco.Security;

public class UnauthorizedReason : Lookup {
    public UnauthorizedReason(string id, HttpStatusCode statusCode) : base(id) {
        StatusCode = (int)statusCode;
    }
    
    public int StatusCode { get; }
}

[StaticLookups]
public class UnauthorizedReasons : StaticLookupsCollection<UnauthorizedReason> {
    public static readonly UnauthorizedReason InsufficientPermissions = new("insufficientPermissions", HttpStatusCode.Unauthorized);
    public static readonly UnauthorizedReason RestrictionsNotMet = new("restrictionsNotMet", HttpStatusCode.Unauthorized);
    public static readonly UnauthorizedReason UserAccessDenied = new("userAccessDenied", HttpStatusCode.Forbidden);
}

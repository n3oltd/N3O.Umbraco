using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Newsletters.SendGrid.Lookups; 

public class ApiError : Lookup {
    public ApiError(string id) : base(id) { }
}

[StaticLookups]
public class ApiErrors : StaticLookupsCollection<ApiError> {
    public static readonly ApiError InvalidApiKey = new("invalidApiKey");
    public static readonly ApiError ListNotFound = new("listNotFound");
}
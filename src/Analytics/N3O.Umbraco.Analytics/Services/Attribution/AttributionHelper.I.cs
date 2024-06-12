using Microsoft.AspNetCore.Http;

namespace N3O.Umbraco.Analytics.Services;

public interface IAttributionHelper {
    void AddOrUpdateAttributionCookie(HttpContext context);
    bool HasAttributions(IQueryCollection queryParameters);
}
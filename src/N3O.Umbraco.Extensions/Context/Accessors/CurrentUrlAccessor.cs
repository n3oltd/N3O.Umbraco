using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace N3O.Umbraco.Context;

public class CurrentUrlAccessor : ICurrentUrlAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUrlAccessor(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetDisplayUrl() {
        return _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
    }

    public string GetEncodedUrl() {
        return _httpContextAccessor.HttpContext?.Request.GetEncodedUrl();
    }
}

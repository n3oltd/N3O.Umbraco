using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Context;

public class QueryStringAccessor : IQueryStringAccessor {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QueryStringAccessor(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetValue(string name) {
        var query = _httpContextAccessor.HttpContext?.Request.Query;
        var key = GetKey(name);
        
        if (key != null) {
            return query[key];
        } else {
            return null;
        }
    }

    public bool Has(string name) {
        var key = GetKey(name);

        return key != null;
    }

    private string GetKey(string name) {
        var query = _httpContextAccessor.HttpContext?.Request.Query;
        var key = query?.Keys.SingleOrDefault(x => x.EqualsInvariant(name));

        return key;
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Parameters;

public class HttpParameterDataSource : IParameterDataSource {
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpParameterDataSource(IHttpContextAccessor contextAccessor) {
        _contextAccessor = contextAccessor;
    }

    public IReadOnlyDictionary<string, string> GetData() {
        var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        if (_contextAccessor.HttpContext != null) {
            foreach (var (key, value) in _contextAccessor.HttpContext.Request.OrEmpty(x => x.Query)) {
                dict[key] = value;
            }
            
            var routeData = _contextAccessor.HttpContext.GetRouteData().Values;

            foreach (var (key, value) in routeData.OrEmpty()) {
                dict[key] = value?.ToString();
            }
        }

        return dict;
    }

    public long Order => 2;
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Parameters {
    public class HttpParameterDataSource : IParameterDataSource {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpParameterDataSource(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public IReadOnlyDictionary<string, string> GetData() {
            var dict = new Dictionary<string, string>();

            if (_contextAccessor.HttpContext != null) {
                var routeData = _contextAccessor.HttpContext?.GetRouteData().Values;

                foreach (var (key, value) in routeData.OrEmpty()) {
                    dict[key] = value?.ToString();
                }
            }

            return dict;
        }

        public long Order => 2;
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace N3O.Umbraco.Hosting {
    public abstract class RouteConstraint : IRouteConstraint {
        public bool Match(HttpContext httpContext,
                          IRouter route,
                          string routeKey,
                          RouteValueDictionary values,
                          RouteDirection routeDirection) {
            if (values.TryGetValue(routeKey, out var objValue) && objValue is string strValue) {
                return IsValid(strValue);
            }

            return false;
        }

        protected abstract bool IsValid(string value);
    }
}

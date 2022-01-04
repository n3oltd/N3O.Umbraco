using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core;

namespace N3O.Umbraco.Hosting {
    public class ActionLinkGenerator : IActionLinkGenerator {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public ActionLinkGenerator(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) {
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
        }

        public string GetUrl<TController>(Expression<Func<TController, object>> methodSelector,
                                          object routeValues = null) {
            var method = ExpressionHelper.GetMethodInfo(methodSelector);

            if (method == null) {
                throw new MissingMethodException($"Could not find the method {methodSelector} on type {typeof(TController)}");
            }

            var httpContext = _httpContextAccessor.HttpContext;

            var url = _linkGenerator.GetUriByAction(httpContext, method.Name, typeof(TController).Name, routeValues);

            return url;
        }
    }
}
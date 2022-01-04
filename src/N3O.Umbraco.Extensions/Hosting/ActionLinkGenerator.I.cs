using System;
using System.Linq.Expressions;

namespace N3O.Umbraco.Hosting {
    public interface IActionLinkGenerator {
        string GetUrl<TController>(Expression<Func<TController, object>> methodSelector, object routeValues = null);
    }
}
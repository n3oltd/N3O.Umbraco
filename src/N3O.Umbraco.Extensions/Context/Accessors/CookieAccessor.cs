using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Context {
    public class CookieAccessor : ICookieAccessor {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieAccessor(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
    
        public string GetValue(string name) {
            var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
            var key = cookies?.Keys.SingleOrDefault(x => x.EqualsInvariant(name));

            if (key != null) {
                return cookies[key];
            } else {
                return null;
            }
        }
    }
}

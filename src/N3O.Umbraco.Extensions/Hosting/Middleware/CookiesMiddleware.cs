using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting {
    public class CookiesMiddleware : IMiddleware {
        private readonly IReadOnlyList<ICookie> _cookies;

        public CookiesMiddleware(IEnumerable<ICookie> cookies) {
            _cookies = cookies.OrEmpty().ToList();
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
            context.Response.OnStarting(() => WriteCookiesAsync(context.Response.Cookies));
        
            await next(context);
        }

        private Task WriteCookiesAsync(IResponseCookies responseCookies) {
            foreach (var cookie in _cookies) {
                cookie.Write(responseCookies);
            }
            
            return Task.CompletedTask;
        }
    }
}
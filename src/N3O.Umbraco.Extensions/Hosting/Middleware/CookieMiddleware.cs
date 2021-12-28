using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public abstract class CookieMiddleware : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        context.Response.OnStarting(() => ModifyCookiesAsync(context.Response.Cookies));
        
        await next(context);
    }

    protected abstract Task ModifyCookiesAsync(IResponseCookies cookies);
}
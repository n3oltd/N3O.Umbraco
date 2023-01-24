using Flurl;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Blazor.Hosting; 

public class BlazorAssetsMiddleware : IMiddleware {
    private static readonly string[] Patterns = {
        "/_content/", "/_blazor/"
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var path = context.Request.Path.ToString();

        var pattern = Patterns.FirstOrDefault(x => path.InvariantContains(x));

        if (pattern.HasValue() && !path.StartsWith(pattern)) {
            var blazorAsset = path.Substring(path.IndexOf(pattern));

            var url = new Url(context.Request.Uri());
            url.ResetToRoot();
            url.AppendPathSegment(blazorAsset);

            context.Response.Redirect(url);

            return;
        }

        await next(context);
    }
}
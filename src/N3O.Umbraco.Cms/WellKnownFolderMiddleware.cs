using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco;

public class WellKnownFolderMiddleware {
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public WellKnownFolderMiddleware(RequestDelegate next, IHostEnvironment env) {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context) {
        var requestPath = context.Request.Path.Value;

        if (requestPath != null && requestPath.StartsWith("/.well-known/", System.StringComparison.OrdinalIgnoreCase)) {
            var relativePath = requestPath.Substring(1).Replace('/', Path.DirectorySeparatorChar);
            var wellKnownRoot = Path.Combine(_env.ContentRootPath, "wwwroot", ".well-known");
            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot", relativePath);

            if (!Path.GetFullPath(fullPath).StartsWith(wellKnownRoot)) {
                context.Response.StatusCode = 403;
                return;
            }

            if (!File.Exists(fullPath)) {
                context.Response.StatusCode = 404;
                return;
            }

            var contentType = GetContentType(fullPath);
            context.Response.ContentType = contentType;

            await context.Response.SendFileAsync(fullPath);
            return;
        }

        await _next(context);
    }
    
    private string GetContentType(string filePath) {
        var ext = Path.GetExtension(filePath).ToLowerInvariant();
        return ext switch {
                   ".txt" => "text/plain",
                   _ => "application/json"
               };
    }
}
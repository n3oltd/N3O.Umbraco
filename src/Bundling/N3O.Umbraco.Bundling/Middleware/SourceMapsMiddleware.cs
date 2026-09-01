using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Bundling.Middleware;

// A build that emits sourcemaps for staging produces .map files in the deployed artifact, and static
// files would serve them to anyone who asks. This withholds them when the host has not opted in. It
// must run before UseStaticFiles, so it is registered through an IStartupFilter.
public class SourceMapsMiddleware {
    private readonly RequestDelegate _next;
    private readonly PathString _assetsPath;

    public SourceMapsMiddleware(RequestDelegate next, BundlingSettings settings) {
        _next = next;
        _assetsPath = GetAssetsPath(settings);
    }

    public async Task InvokeAsync(HttpContext context) {
        var path = context.Request.Path;

        if (path.HasValue &&
            path.StartsWithSegments(_assetsPath) &&
            path.Value.EndsWith(".map", StringComparison.OrdinalIgnoreCase)) {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        await _next(context);
    }

    private static PathString GetAssetsPath(BundlingSettings settings) {
        var directory = Path.GetDirectoryName(settings.ManifestPath) ?? string.Empty;
        var normalised = directory.Replace('\\', '/').Trim('/');

        return new PathString($"/{normalised}");
    }
}

using HeyRed.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public class WellKnownFolderMiddleware : IMiddleware {
    private const string Prefix = "/.well-known/";
    private const string Root = "wellKnownRoot";
    
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRequestHostAccessor _requestHostAccessor;

    public WellKnownFolderMiddleware(IWebHostEnvironment webHostEnvironment, IRequestHostAccessor requestHostAccessor) {
        _webHostEnvironment = webHostEnvironment;
        _requestHostAccessor = requestHostAccessor;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var requestPath = context.Request.Path.Value;

        if (requestPath != null &&
            !requestPath.Contains("..") &&
            requestPath.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase)) {
            var path = requestPath.Substring(Prefix.Length);
            
            var filePaths = new[] {
                Path.Combine(_webHostEnvironment.WebRootPath, Root, _requestHostAccessor.GetHost(), path),
                Path.Combine(_webHostEnvironment.WebRootPath, Root, path)
            };

            var filePath = filePaths.FirstOrDefault(File.Exists);

            if (filePath.HasValue()) {
                context.Response.ContentType = MimeTypesMap.GetMimeType(filePath);

                await context.Response.SendFileAsync(filePath);
            } else {
                context.Response.StatusCode = 404;
            }
        } else {
            await next(context);
        }
    }
}

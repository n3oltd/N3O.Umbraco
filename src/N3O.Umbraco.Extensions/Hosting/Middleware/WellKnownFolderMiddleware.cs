using HeyRed.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public class WellKnownFolderMiddleware : IMiddleware {
    private const string Prefix = "/.well-known/";
    
    private readonly IWebHostEnvironment _webHostEnvironment;

    public WellKnownFolderMiddleware(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var requestPath = context.Request.Path.Value;

        if (requestPath != null &&
            !requestPath.Contains("..") &&
            requestPath.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase)) {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, requestPath);

            if (File.Exists(filePath)) {
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
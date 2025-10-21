using HeyRed.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms.Hosting;

public class PlatformsTemplatesMiddleware : IMiddleware {
    private const string Prefix = "/platforms/templates/";
    
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PlatformsTemplatesMiddleware(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var requestPath = context.Request.Path.Value;

        if (requestPath != null &&
            !requestPath.Contains("..") &&
            requestPath.StartsWith(Prefix, StringComparison.InvariantCultureIgnoreCase)) {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, requestPath.TrimStart('/'));

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
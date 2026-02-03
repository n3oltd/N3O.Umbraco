using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class ConnectMiddleware : IMiddleware {
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly ICloudUrl _cloudUrl;

    public ConnectMiddleware(ISubscriptionAccessor subscriptionAccessor, ICloudUrl cloudUrl) {
        _subscriptionAccessor = subscriptionAccessor;
        _cloudUrl = cloudUrl;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        var requestPath = context.Request.Path.Value;
        var subscription = _subscriptionAccessor.GetSubscription();

        var connectPath = subscription.HasValue() ? $"/connect-{subscription.Id.Code}/" : null;
        
        if (requestPath != null &&
            connectPath != null &&
            !requestPath.Contains("..") &&
            requestPath.StartsWith(connectPath, StringComparison.InvariantCultureIgnoreCase)) {
            try {
                
                var cdnPath = requestPath.Substring(connectPath.Length);
                var cdnUrl = _cloudUrl.ForCdn(CdnRoots.Connect, cdnPath);
                
                using (var httpClient = new HttpClient()) {
                    var httpResponse = await httpClient.GetAsync(cdnUrl);

                    httpResponse.EnsureSuccessStatusCode();

                    context.Response.ContentType = MimeTypesMap.GetMimeType(cdnPath);
                    
                    await context.Response.BodyWriter.WriteAsync(await httpResponse.Content.ReadAsByteArrayAsync());
                }
            } catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound) {
                context.Response.StatusCode = 404;
            }
        } else {
            await next(context);
        }
    }
}
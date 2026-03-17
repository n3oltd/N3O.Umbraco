using HeyRed.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Extensions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Hosting;

public class ConnectMiddleware : IMiddleware {
    private readonly IMemoryCache _cache;
    
    private readonly ISubscriptionAccessor _subscriptionAccessor;
    private readonly ICloudUrl _cloudUrl;

    public ConnectMiddleware(ISubscriptionAccessor subscriptionAccessor,
                             ICloudUrl cloudUrl,
                             IMemoryCache cache) {
        _subscriptionAccessor = subscriptionAccessor;
        _cloudUrl = cloudUrl;
        _cache = cache;
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
                
                var data = await _cache.GetOrCreateAsync(cdnUrl, async entry => {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

                    using (var httpClient = new HttpClient()) {
                        var httpResponse = await httpClient.GetAsync(cdnUrl);

                        httpResponse.EnsureSuccessStatusCode();
                        
                        return await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                });
                
                context.Response.ContentType = MimeTypesMap.GetMimeType(cdnPath);
                context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
                    
                await context.Response.BodyWriter.WriteAsync(data);
            } catch (HttpRequestException e) when (e.StatusCode == HttpStatusCode.NotFound) {
                context.Response.StatusCode = 404;
            }
        } else {
            await next(context);
        }
    }
}
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = N3O.Umbraco.Cloud.Lookups.JsonSerializer;

namespace N3O.Umbraco.Cloud;

public class CdnClient : ICdnClient {
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);
    private static readonly MemoryCache ContentCache = new(new MemoryCacheOptions());
    private static readonly ConcurrentDictionary<string, string> MostRecentDownloads = new();
    
    private readonly ICloudUrl _cloudUrl;
    private readonly IJsonProvider _jsonProvider;
    private readonly ILogger<CdnClient> _logger;

    public CdnClient(ICloudUrl cloudUrl, IJsonProvider jsonProvider, ILogger<CdnClient> logger) {
        _cloudUrl = cloudUrl;
        _jsonProvider = jsonProvider;
        _logger = logger;
    }

    public async Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                                          string path,
                                                          JsonSerializer jsonSerializer,
                                                          CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(kind, path);

        var res = await ContentCache.GetOrCreateAsync(GetCacheKey(publishedUrl, typeof(T).FullName), async c => {
            c.AbsoluteExpirationRelativeToNow = CacheDuration;

            var json = await DownloadStringAsync(publishedUrl, cancellationToken);
            
            return json.IfNotNull(x => Deserialize<T>(x, jsonSerializer));
        });

        return res;
    }

    public async Task<PublishedContentResult> DownloadPublishedContentAsync(string path,
                                                                            CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(path);

        var res = await ContentCache.GetOrCreateAsync(GetCacheKey(publishedUrl, typeof(object).FullName), async c => {
            c.AbsoluteExpirationRelativeToNow = CacheDuration;
            
            var json = await DownloadStringAsync(publishedUrl, cancellationToken);

            var jObject = json.IfNotNull(JObject.Parse);
            var kind = jObject.GetPublishedFileKind();
            
            if (kind.HasValue()) {
                Guid.TryParse(jObject["id"]?.ToString(), out var id);
                
                return PublishedContentResult.ForFound(id, kind, path, jObject);
            } else {
                return PublishedContentResult.ForNotFound(path);
            }
        });

        return res;
    }

    private async Task<string> DownloadStringAsync(string publishedUrl, CancellationToken cancellationToken) {
        string download;
            
        try {
            using (var httpClient = new HttpClient()) {
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                    
                download = await httpClient.GetStringAsync(publishedUrl, cancellationToken);

                MostRecentDownloads[publishedUrl] = download; 
            }
        } catch(Exception ex) {
            _logger.LogError(ex, "Could not download {PublishedUrl}", publishedUrl);
            
            download = MostRecentDownloads.GetOrDefault(publishedUrl);
        }
            
        return download;
    }
    
    private string GetPublishedContentUrl(PublishedFileKind kind, string path) {
        return GetPublishedContentUrl(GetPublishedPath(kind, path));
    }

    private T Deserialize<T>(string json, JsonSerializer jsonSerializer) {
        if (jsonSerializer == JsonSerializers.JsonProvider) {
            return _jsonProvider.DeserializeObject<T>(json);
        } else if (jsonSerializer == JsonSerializers.Simple) {
            return JsonConvert.DeserializeObject<T>(json);
        } else {
            throw UnrecognisedValueException.For(jsonSerializer);
        }
    }
    
    private string GetCacheKey(string publishedUrl, string type) {
        return $"{nameof(CdnClient)}_{publishedUrl}_{type}";
    }
    
    private string GetPublishedPath(PublishedFileKind kind, string path) {
        return $"{kind.PathSegment}/{path}";
    }
    
    private string GetPublishedContentUrl(string path) {
        return _cloudUrl.ForCdn(CdnRoots.Connect, path);
    }
}
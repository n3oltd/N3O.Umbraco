using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = N3O.Umbraco.Cloud.Lookups.JsonSerializer;

namespace N3O.Umbraco.Cloud;

public class CdnClient : ICdnClient {
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);
    private static readonly MemoryCache ContentCache = new(new MemoryCacheOptions());
    
    private readonly ICloudUrl _cloudUrl;
    private readonly IJsonProvider _jsonProvider;

    public CdnClient(ICloudUrl cloudUrl, IJsonProvider jsonProvider) {
        _cloudUrl = cloudUrl;
        _jsonProvider = jsonProvider;
    }

    public async Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                                          string path,
                                                          JsonSerializer jsonSerializer,
                                                          CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(kind, path);

        var res = await ContentCache.GetOrCreateAsync(GetCacheKey(publishedUrl, typeof(T).FullName), async c => {
            c.AbsoluteExpirationRelativeToNow = CacheDuration;

            try {
                using (var httpClient = new HttpClient()) {
                    var json = await httpClient.GetStringAsync(publishedUrl, cancellationToken);

                    return Deserialize<T>(json, jsonSerializer);
                }
            } catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                return default;
            }
        });

        return res;
    }

    public async Task<PublishedContentResult> DownloadPublishedContentAsync(string path,
                                                                            CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(path);

        var res = await ContentCache.GetOrCreateAsync(GetCacheKey(publishedUrl, typeof(object).FullName), async c => {
            c.AbsoluteExpirationRelativeToNow = CacheDuration;

            try {
                using (var httpClient = new HttpClient()) {
                    var json = await httpClient.GetStringAsync(publishedUrl, cancellationToken);

                    var jObject = JObject.Parse(json);
                    var kindId = jObject["kind"]?.ToString();
                    var kind = StaticLookups.FindById<PublishedFileKind>(kindId);
                    
                    Guid.TryParse(jObject["id"]?.ToString(), out var id);

                    if (kind.HasValue()) {
                        return PublishedContentResult.ForFound(id, kind, path, jObject);
                    } else {
                        return PublishedContentResult.ForNotFound(path);
                    }
                }
            } catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                return PublishedContentResult.ForNotFound(path);
            }
        });

        return res;
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
using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = N3O.Umbraco.Cloud.Lookups.JsonSerializer;

namespace N3O.Umbraco.Cloud;

public class CdnClient : ICdnClient {
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
                                                          CancellationToken cancellationToken) {
        var url = GetPublishedContentUrl(kind, path);

        var res = await ContentCache.GetOrCreateAsync(url, async c => {
            c.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            try {
                using (var httpClient = new HttpClient()) {
                    var json = await httpClient.GetStringAsync(url, cancellationToken);

                    return Deserialize<T>(json, jsonSerializer);
                }
            } catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                return default;
            }
        });

        return res;
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

    public string GetPublishedContentUrl(PublishedFileKind kind, string path) {
        return _cloudUrl.ForCdn(CdnRoots.Connect, $"{kind.PathSegment}/{path}");
    }
}
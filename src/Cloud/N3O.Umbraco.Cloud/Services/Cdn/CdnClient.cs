using Microsoft.Extensions.Caching.Memory;
using N3O.Umbraco.Cloud.Lookups;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class CdnClient : ICdnClient {
    private static readonly MemoryCache ContentCache = new(new MemoryCacheOptions());
    
    private readonly ICloudUrl _cloudUrl;

    public CdnClient(ICloudUrl cloudUrl) {
        _cloudUrl = cloudUrl;
    }
    
    public async Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                                          string path,
                                                          CancellationToken cancellationToken) {
        var url = GetPublishedContentUrl(kind, path);

        var res = await ContentCache.GetOrCreateAsync(url, async c => {
            c.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            
            using (var httpClient = new HttpClient()) {
                var publishedContent = await httpClient.GetStringAsync(url, cancellationToken);

                return JsonConvert.DeserializeObject<T>(publishedContent);
            }
        });

        return res;
    }

    public string GetPublishedContentUrl(PublishedFileKind kind, string path) {
        return _cloudUrl.ForCdn(CdnRoots.Connect, $"{kind.PathSegment}/{path}");
    }
}
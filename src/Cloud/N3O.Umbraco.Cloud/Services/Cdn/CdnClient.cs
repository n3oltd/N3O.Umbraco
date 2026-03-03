using AsyncKeyedLock;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JsonSerializer = N3O.Umbraco.Cloud.Lookups.JsonSerializer;

namespace N3O.Umbraco.Cloud;

public class CdnClient : ICdnClient {
    private static readonly ConcurrentDictionary<string, CdnDownloadResult> Downloads = new(StringComparer.InvariantCultureIgnoreCase);
    
    private readonly ICloudUrl _cloudUrl;
    private readonly IClock _clock;
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;
    private readonly ILogger<CdnClient> _logger;
    private readonly HttpClient _httpClient;

    public CdnClient(ICloudUrl cloudUrl,
                     IClock clock,
                     IJsonProvider jsonProvider,
                     AsyncKeyedLocker<string> locker,
                     ILogger<CdnClient> logger) {
        _cloudUrl = cloudUrl;
        _clock = clock;
        _jsonProvider = jsonProvider;
        _locker = locker;
        _logger = logger;
        
         var handler = new SocketsHttpHandler {
            ConnectCallback = async (context, cancellationToken) => {
                var addresses = await Dns.GetHostAddressesAsync(context.DnsEndPoint.Host, cancellationToken);
                var ipv4Address = addresses.First(a => a.AddressFamily == AddressFamily.InterNetwork);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await socket.ConnectAsync(ipv4Address, context.DnsEndPoint.Port, cancellationToken);

                return new NetworkStream(socket, ownsSocket: true);
            }
        };
        
        _httpClient = new HttpClient(handler);
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
    }

    public async Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                                          string path,
                                                          JsonSerializer jsonSerializer,
                                                          CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(kind, path);

        var json = await FetchStringAsync(publishedUrl, cancellationToken);
            
        return json.IfNotNull(x => Deserialize<T>(x, jsonSerializer));
    }

    public async Task<PublishedContentResult> DownloadPublishedContentAsync(string path,
                                                                            CancellationToken cancellationToken = default) {
        var publishedUrl = GetPublishedContentUrl(path);

        var json = await FetchStringAsync(publishedUrl, cancellationToken);

        var jObject = json.IfNotNull(JObject.Parse);
        var kind = jObject.GetPublishedFileKind();
            
        if (kind.HasValue()) {
            Guid.TryParse(jObject["id"]?.ToString(), out var id);
                
            return PublishedContentResult.ForFound(id, kind, path, jObject);
        } else {
            return PublishedContentResult.ForNotFound(path);
        }
    }

    private async Task<string> FetchStringAsync(string publishedUrl, CancellationToken cancellationToken) {
        using (await _locker.LockAsync(publishedUrl, cancellationToken)) {
            var download = Downloads.GetOrDefault(publishedUrl);
            
            if (download == null || download.IsExpired(_clock) || download.CanRetry(_clock)) {
                Downloads[publishedUrl] = await DownloadStringAsync(publishedUrl, cancellationToken);
            }

            return Downloads[publishedUrl].Content;
        }
    }
    
    private async Task<CdnDownloadResult> DownloadStringAsync(string publishedUrl,
                                                              CancellationToken cancellationToken) {
        try {
            var download = await _httpClient.GetStringAsync(publishedUrl, cancellationToken);

            return CdnDownloadResult.ForSuccess(_clock, download);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Could not download {PublishedUrl}", publishedUrl);

            return CdnDownloadResult.ForFailure(_clock);
        }
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
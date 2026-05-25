using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public interface ICdnClient {
    Task<string> DownloadAsync(string path,
                               bool logNotFound = true,
                               CancellationToken cancellationToken = default);
    
    Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                             string path,
                                             JsonSerializer jsonSerializer,
                                             bool logNotFound = true,
                                             CancellationToken cancellationToken = default);
    
    Task<PublishedContentResult> DownloadPublishedContentAsync(string path,
                                                               bool logNotFound = true,
                                                               CancellationToken cancellationToken = default);
}
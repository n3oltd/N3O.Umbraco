using N3O.Umbraco.Cloud.Lookups;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public interface ICdnClient {
    Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                             string path,
                                             JsonSerializer jsonSerializer,
                                             CancellationToken cancellationToken);
    string GetPublishedContentUrl(PublishedFileKind kind, string path);
}
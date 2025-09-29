using N3O.Umbraco.Cloud.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public interface ICdnClient {
    Task<T> DownloadPublishedContentAsync<T>(PublishedFileKind kind,
                                             string path,
                                             JsonSerializer jsonSerializer,
                                             CancellationToken cancellationToken = default);
    
    Task<(Guid, PublishedFileKind, IReadOnlyDictionary<string, object>)> DownloadPublishedContentAsync(string publishedPath,
                                                                                                       CancellationToken cancellationToken = default);
    
    Task<(Guid, PublishedFileKind, IReadOnlyDictionary<string, object>)> DownloadPublishedPageAsync(string path, 
                                                                                                    CancellationToken cancellationToken = default);
    
    string GetPublishedContentUrl(PublishedFileKind kind, string path);
}
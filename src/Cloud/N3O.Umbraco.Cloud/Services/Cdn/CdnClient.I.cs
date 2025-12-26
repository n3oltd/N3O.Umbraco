using N3O.Umbraco.Cloud.Lookups;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public interface ICdnClient {
    // TODO Swap out string kind for enum
    Task<T> DownloadPublishedContentAsync<T>(string kind,
                                             string path,
                                             JsonSerializer jsonSerializer,
                                             CancellationToken cancellationToken = default);
    
    Task<(Guid, string, IReadOnlyDictionary<string, object>)> DownloadPublishedContentAsync(string publishedPath,
                                                                                            CancellationToken cancellationToken = default);
    
    Task<(Guid, string, IReadOnlyDictionary<string, object>)> DownloadPublishedPageAsync(string kind,
                                                                                         string path, 
                                                                                         CancellationToken cancellationToken = default);
    
    string GetPublishedContentUrl(string kind, string path);
}
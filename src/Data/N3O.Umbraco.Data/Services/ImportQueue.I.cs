using N3O.Umbraco.Data.Lookups;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data; 

public interface IImportQueue {
    Task AppendAsync(Guid containerId,
                     string contentTypeAlias,
                     string filename,
                     int? row,
                     Instant queuedAt,
                     string queuedBy,
                     DatePattern datePattern,
                     string storageFolderName,
                     Guid? contentId,
                     string replacesCriteria,
                     string name,
                     bool moveUpdatedContentToContainer,
                     IReadOnlyDictionary<string, string> sourceValues,
                     CancellationToken cancellationToken = default);

    Task<int> CommitAsync(bool queueForProcessing = true);
}
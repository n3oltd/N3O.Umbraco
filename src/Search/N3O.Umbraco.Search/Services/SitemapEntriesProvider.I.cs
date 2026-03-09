using N3O.Umbraco.Search.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search;

public interface ISitemapEntriesProvider {
    Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default);
}

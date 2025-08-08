using N3O.Umbraco.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search;

public interface ISitemap {
    IReadOnlyList<SitemapEntry> GetEntries();
    Task PublishAsync();
}

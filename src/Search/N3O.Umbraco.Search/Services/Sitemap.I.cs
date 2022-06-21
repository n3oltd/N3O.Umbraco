using N3O.Umbraco.Search.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Search;

public interface ISitemap {
    IReadOnlyList<SitemapEntry> GetEntries();
    string GetXml();
}

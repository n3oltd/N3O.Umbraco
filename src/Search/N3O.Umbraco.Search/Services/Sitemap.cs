using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Search.Extensions;
using N3O.Umbraco.Search.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search;

public class Sitemap : ISitemap {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IReadOnlyList<ISitemapEntriesProvider> _entriesProviders;

    public Sitemap(IWebHostEnvironment webHostEnvironment, IEnumerable<ISitemapEntriesProvider> entriesProviders) {
        _webHostEnvironment = webHostEnvironment;
        _entriesProviders = entriesProviders.ApplyAttributeOrdering();
    }

    public async Task<IReadOnlyList<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        var entries = new List<SitemapEntry>();

        foreach (var provider in _entriesProviders) {
            entries.AddRange(await provider.GetEntriesAsync(cancellationToken));
        }

        return entries;
    }

    public async Task PublishAsync() {
        var sitemapXml = await GetXmlAsync();

        await WebRoot.SaveTextAsync(_webHostEnvironment, "sitemap.xml", sitemapXml);
    }

    private async Task<string> GetXmlAsync() {
        var entries = await GetEntriesAsync();

        return entries.ToXml();
    }
}

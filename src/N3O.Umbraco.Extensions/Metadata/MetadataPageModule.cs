using N3O.Umbraco.Constants;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Metadata;

public class MetadataPageModule : IPageModule {
    private readonly IEnumerable<IMetadataProvider> _allProviders;

    public MetadataPageModule(IEnumerable<IMetadataProvider> allProviders) {
        _allProviders = allProviders;
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var entries = new List<MetadataEntry>();

        foreach (var provider in _allProviders) {
            if (await provider.IsProviderForAsync(page)) {
                var newEntries = await provider.GetEntriesAsync(page);

                entries.AddRange(newEntries);
            }
        }

        return Task.FromResult<object>(entries);
    }

    public string Key => PageModules.Keys.MetadataEntries;
}

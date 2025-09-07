using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Linq;
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

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var providers = _allProviders.OrEmpty().Where(x => x.IsProviderFor(page)).ToList();
        var entries = providers.SelectListAsync(x => x.GetEntriesAsync(page));

        return Task.FromResult<object>(entries);
    }

    public string Key => PageModules.Keys.MetadataEntries;
}

using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.OpenGraph;

public class OpenGraphPageModule : IPageModule {
    private readonly IOpenGraphBuilder _openGraphBuilder;
    private readonly IReadOnlyList<IOpenGraphProvider> _allProviders;

    public OpenGraphPageModule(IOpenGraphBuilder openGraphBuilder, IEnumerable<IOpenGraphProvider> allProviders) {
        _openGraphBuilder = openGraphBuilder;
        _allProviders = allProviders.ApplyAttributeOrdering();
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var providers = _allProviders.OrEmpty().Where(x => x.IsProviderFor(page)).ToList();

        foreach (var provider in providers) {
            await provider.AddOpenGraphAsync(_openGraphBuilder, page);
        }

        if (_openGraphBuilder.HasData) {
            return _openGraphBuilder.Build();
        } else {
            return null;
        }
    }

    public string Key => PageModules.Keys.OpenGraph;
}

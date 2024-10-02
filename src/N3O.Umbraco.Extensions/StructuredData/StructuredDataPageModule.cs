using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData;

public class StructuredDataPageModule : IPageModule {
    private readonly IEnumerable<IStructuredDataProvider> _allProviders;

    public StructuredDataPageModule(IEnumerable<IStructuredDataProvider> allProviders) {
        _allProviders = allProviders;
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var providers = _allProviders.OrEmpty().Where(x => x.IsProviderFor(page)).ToList();

        var root = JsonLd.Root();

        foreach (var provider in providers) {
            provider.AddStructuredData(root, page);
        }

        var javaScript = JsonConvert.SerializeObject(root, Formatting.Indented);

        return Task.FromResult<object>(new StructuredDataCode(javaScript.ToHtmlString()));
    }

    public string Key => PageModules.Keys.StructuredData;
}

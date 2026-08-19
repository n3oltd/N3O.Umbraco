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

        var nodes = new List<JsonLd>();

        foreach (var provider in providers) {
            var node = JsonLd.New();

            provider.AddStructuredData(node, page);

            if (node.Count > 0) {
                nodes.Add(node);
            }
        }

        var root = JsonLd.Root();

        if (nodes.Count == 1) {
            foreach (var entry in nodes[0]) {
                root[entry.Key] = entry.Value;
            }
        } else if (nodes.Count > 1) {
            root.Custom("@graph", nodes);
        }

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

        var javaScript = JsonConvert.SerializeObject(root, Formatting.Indented, serializerSettings);

        return Task.FromResult<object>(new StructuredDataCode(javaScript.ToHtmlString()));
    }

    public string Key => PageModules.Keys.StructuredData;
}

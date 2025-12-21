using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Templates.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates.Modules;

public class MergeModelsPageModule : IPageModule {
    private readonly ConcurrentDictionary<IPublishedContent, IReadOnlyDictionary<string, object>> _mergeModelsCache = new();
    private readonly IReadOnlyList<IMergeModelProvider> _mergeModelProviders;

    public MergeModelsPageModule(IEnumerable<IMergeModelProvider> mergeModelProviders) {
        _mergeModelProviders = mergeModelProviders.ApplyAttributeOrdering();
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public async Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var mergeModels = await _mergeModelProviders.GetMergeModelsAsync(page, _mergeModelsCache);
        
        return mergeModels;
    }

    public string Key => TemplateConstants.PageModuleKeys.MergeModels;
}

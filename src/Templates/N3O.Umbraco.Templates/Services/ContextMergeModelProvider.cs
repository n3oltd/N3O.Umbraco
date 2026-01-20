using N3O.Umbraco.Localization;
using N3O.Umbraco.Templates.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public class ContextMergeModelProvider : MergeModelsProvider {
    private readonly ILocalClock _localClock;

    public ContextMergeModelProvider(ILocalClock localClock) {
        _localClock = localClock;
    }

    protected override Task PopulateModelsAsync(IPublishedContent content,
                                                Dictionary<string, object> mergeModels,
                                                CancellationToken cancellationToken = default) {
        var now = _localClock.GetLocalNow();
        
        var model = new ContextMergeModel();
        model.Date = now.Date;
        model.Time = now.TimeOfDay;

        mergeModels["context"] = model;
        
        return Task.CompletedTask;
    }
}
using N3O.Umbraco.Localization;
using N3O.Umbraco.Templates.Models;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public class ContextMergeModelProvider : MergeModelProvider<ContextMergeModel> {
    private readonly ILocalClock _localClock;

    public ContextMergeModelProvider(ILocalClock localClock) {
        _localClock = localClock;
    }
    
    protected override Task<ContextMergeModel> GetModelAsync(IPublishedContent content,
                                                             CancellationToken cancellationToken) {
        var now = _localClock.GetLocalNow();
        
        var model = new ContextMergeModel();
        model.Date = now.Date;
        model.Time = now.TimeOfDay;
        
        return Task.FromResult(model);
    }

    public override string Key => "context";
}
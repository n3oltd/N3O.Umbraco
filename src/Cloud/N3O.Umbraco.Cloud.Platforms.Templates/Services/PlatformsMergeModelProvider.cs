using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public abstract class PlatformsMergeModelProvider : MergeModelProvider<IReadOnlyDictionary<string, object>> {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    protected PlatformsMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }

    public override async Task<bool> IsProviderForAsync(IPublishedContent content) {
        var platformsPage = await _platformsPageAccessor.GetAsync();

        return platformsPage?.Kind == Kind;
    }

    protected override async Task<IReadOnlyDictionary<string, object>> GetModelAsync(IPublishedContent content,
                                                                                     CancellationToken cancellationToken) {
        var platformsPage = await _platformsPageAccessor.GetAsync();
        
        return platformsPage.MergeModel;
    }
    
    public override string Key => Kind.Id;
    
    protected abstract PublishedFileKind Kind { get; }
}
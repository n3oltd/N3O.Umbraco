using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

// + account merge model provider working the same way, but be sure to "forward" the n3o-account header if they have
// passed a header selecting a specific account
public class PlatformUserMergeModelProvider : MergeModelProvider<IReadOnlyDictionary<string, object>> {
    private readonly IPlatformsPageAccessor _platformsPageAccessor;

    protected PlatformUserMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor) {
        _platformsPageAccessor = platformsPageAccessor;
    }

    public override async Task<bool> IsProviderForAsync(IPublishedContent content) {
        var platformsPage = await _platformsPageAccessor.GetAsync();

        return platformsPage.HasValue();
    }

    protected override async Task<IReadOnlyDictionary<string, object>> GetModelAsync(IPublishedContent content,
                                                                                     CancellationToken cancellationToken) {
        // Make an API call to connect andcatch any errors, and populate the response into a merge model.
    }
    
    public override string Key => "user";
}
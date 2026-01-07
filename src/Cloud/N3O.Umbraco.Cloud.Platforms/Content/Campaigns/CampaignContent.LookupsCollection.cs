using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

[Order(int.MinValue)]
public class ContentCampaigns : LookupsCollection<Campaign> {
    private readonly IContentCache _contentCache;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public ContentCampaigns(IContentCache contentCache, IUmbracoContextAccessor umbracoContextAccessor) {
        _contentCache = contentCache;
        _umbracoContextAccessor = umbracoContextAccessor;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }
    
    protected override Task<IReadOnlyList<Campaign>> LoadAllAsync(CancellationToken cancellationToken) {
        var all = GetFromCache();
        
        return Task.FromResult(all);
    }

    private IReadOnlyList<Campaign> GetFromCache() {
        List<CampaignContent> content;
        
        if (_umbracoContextAccessor.TryGetUmbracoContext(out _)) {
            content = _contentCache.All<IPublishedContent>(x => x.IsComposedOf(AliasHelper<CampaignContent>.ContentTypeAlias()))
                                   .OrderBy(x => x.Name)
                                   .As<CampaignContent>()
                                   .ToList();
        } else {
            content = [];
        }
        
        var lookups = content.Select(ToCampaign).ToList();

        return lookups;
    }

    private Campaign ToCampaign(CampaignContent campaignContent) {
        return new Campaign(LookupContent.GetId(campaignContent.Content()),
                            LookupContent.GetName(campaignContent.Content()),
                            campaignContent.Content().Key);
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}
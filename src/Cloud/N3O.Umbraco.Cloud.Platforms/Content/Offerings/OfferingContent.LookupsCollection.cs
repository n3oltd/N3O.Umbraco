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
public class ContentOfferings : LookupsCollection<Offering> {
    private readonly IContentCache _contentCache;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public ContentOfferings(IContentCache contentCache, IUmbracoContextAccessor umbracoContextAccessor) {
        _contentCache = contentCache;
        _umbracoContextAccessor = umbracoContextAccessor;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }
    
    protected override Task<IReadOnlyList<Offering>> LoadAllAsync(CancellationToken cancellationToken) {
        var all = GetFromCache();
        
        return Task.FromResult(all);
    }

    private IReadOnlyList<Offering> GetFromCache() {
        List<OfferingContent> content;
        
        if (_umbracoContextAccessor.TryGetUmbracoContext(out _)) {
            content = _contentCache.All<IPublishedContent>(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                   .OrderBy(x => x.Name)
                                   .As<OfferingContent>()
                                   .ToList();
        } else {
            content = [];
        }
        
        var lookups = content.Select(ToOffering).ToList();

        return lookups;
    }

    private Offering ToOffering(OfferingContent offeringContent) {
        return new Offering(LookupContent.GetId(offeringContent.Content()),
                            LookupContent.GetName(offeringContent.Content()),
                            offeringContent.Content().Key);
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}
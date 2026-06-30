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
using Umbraco.Extensions;

[Order(int.MinValue)]
public class ContentOfferings : LookupsCollection<Offering> {
    private readonly IContentCache _contentCache;

    public ContentOfferings(IContentCache contentCache) {
        _contentCache = contentCache;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }

    protected override Task<IReadOnlyList<Offering>> LoadAllAsync(CancellationToken cancellationToken) {
        var all = GetFromCache();

        return Task.FromResult(all);
    }

    private IReadOnlyList<Offering> GetFromCache() {
        var content = _contentCache.All<IPublishedContent>(x => x.IsComposedOf(AliasHelper<OfferingContent>.ContentTypeAlias()))
                                   .OrderBy(x => x.Name)
                                   .As<OfferingContent>()
                                   .ToList();

        var lookups = content.Select(ToOffering).ToList();

        return lookups;
    }

    private Offering ToOffering(OfferingContent offeringContent) {
        return new Offering(LookupContent.GetId(offeringContent.Content()),
                            LookupContent.GetName(offeringContent.Content()),
                            offeringContent.Content().Key,
                            offeringContent.Content().Parent<IPublishedContent>().Key.ToString());
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class QurbaniSeasonContent : UmbracoContent<QurbaniSeasonContent> { }


[Order(int.MinValue)]
public class ContentQurbaniSeasons : LookupsCollection<QurbaniSeason> {
    private readonly IContentCache _contentCache;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public ContentQurbaniSeasons(IContentCache contentCache, IUmbracoContextAccessor umbracoContextAccessor) {
        _contentCache = contentCache;
        _umbracoContextAccessor = umbracoContextAccessor;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }
    
    protected override Task<IReadOnlyList<QurbaniSeason>> LoadAllAsync(CancellationToken cancellationToken) {
        var all = GetFromCache();
        
        return Task.FromResult(all);
    }

    private IReadOnlyList<QurbaniSeason> GetFromCache() {
        List<QurbaniSeasonContent> content;
        
        if (_umbracoContextAccessor.TryGetUmbracoContext(out _)) {
            content = _contentCache.All<QurbaniSeasonContent>().OrderBy(x => x.Content().Name).ToList();
        } else {
            content = [];
        }
        
        var lookups = content.Select(ToQurbaniSeason).ToList();

        return lookups;
    }

    private QurbaniSeason ToQurbaniSeason(QurbaniSeasonContent qurbaniSeasonContent) {
        return new QurbaniSeason(LookupContent.GetId(qurbaniSeasonContent.Content()),
                                 LookupContent.GetName(qurbaniSeasonContent.Content()),
                                 qurbaniSeasonContent.Content().Key);
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}
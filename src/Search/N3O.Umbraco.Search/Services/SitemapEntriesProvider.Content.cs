using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Models;
using NodaTime.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Search;

public class ContentSitemapEntriesProvider : ISitemapEntriesProvider {
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IContentLocator _contentLocator;
    private readonly IContentVisibility _contentVisibility;

    public ContentSitemapEntriesProvider(IUmbracoContextFactory umbracoContextFactory,
                                         IContentLocator contentLocator,
                                         IContentVisibility contentVisibility) {
        _umbracoContextFactory = umbracoContextFactory;
        _contentLocator = contentLocator;
        _contentVisibility = contentVisibility;
    }
    
    public Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            var publicContent = _contentLocator.All()
                                               .Where(x => _contentVisibility.IsVisible(x))
                                               .Select(x => new SitemapEntry(x.AbsoluteUrl(),
                                                                             "daily",
                                                                             0.5f,
                                                                             x.UpdateDate.ToLocalDateTime().Date))
                                               .ToList();

            return Task.FromResult<IEnumerable<SitemapEntry>>(publicContent);
        }
    }
}

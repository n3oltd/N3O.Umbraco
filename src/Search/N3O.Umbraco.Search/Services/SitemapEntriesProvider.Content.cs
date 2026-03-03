using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Search.Models;
using NodaTime.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Search;

public class ContentSitemapEntriesProvider : ISitemapEntriesProvider {
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;
    private readonly IContentLocator _contentLocator;
    private readonly IContentVisibility _contentVisibility;

    public ContentSitemapEntriesProvider(IUmbracoContextFactory umbracoContextFactory,
                                         ILocalizationSettingsAccessor localizationSettingsAccessor,
                                         IContentLocator contentLocator,
                                         IContentVisibility contentVisibility) {
        _umbracoContextFactory = umbracoContextFactory;
        _localizationSettingsAccessor = localizationSettingsAccessor;
        _contentLocator = contentLocator;
        _contentVisibility = contentVisibility;
    }
    
    public Task<IEnumerable<SitemapEntry>> GetEntriesAsync(CancellationToken cancellationToken = default) {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            var localizationSettings = _localizationSettingsAccessor.GetSettings();
            
            var publicContent = _contentLocator.All()
                                               .Where(x => _contentVisibility.IsVisible(x))
                                               .Select(x => GetSitemapEntry(x, localizationSettings.DefaultCultureCode))
                                               .ToList();

            return Task.FromResult<IEnumerable<SitemapEntry>>(publicContent);
        }
    }

    private SitemapEntry GetSitemapEntry(IPublishedContent publishedContent, string defaultCultureCode) {
        var cultureVariantUrls = new Dictionary<string, string>();

        foreach (var cultureCode in publishedContent.OrEmpty(x => x.Cultures).Select(x => x.Key).Except(defaultCultureCode)) {
            cultureVariantUrls[cultureCode] = publishedContent.AbsoluteUrl(cultureCode);
        }
        
        return new SitemapEntry(publishedContent.AbsoluteUrl(),
                                "daily",
                                0.5f,
                                publishedContent.UpdateDate.ToLocalDateTime().Date,
                                cultureVariantUrls);
    }
}

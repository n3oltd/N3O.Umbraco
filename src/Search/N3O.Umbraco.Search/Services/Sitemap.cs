using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Search.Models;
using NodaTime.Extensions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using X.Web.Sitemap;
using X.Web.Sitemap.Extensions;
using XSitemap = X.Web.Sitemap.Sitemap;

namespace N3O.Umbraco.Search;

public class Sitemap : ISitemap {
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentLocator _contentLocator;
    private readonly IContentVisibility _contentVisibility;

    public Sitemap(IUmbracoContextFactory umbracoContextFactory,
                   IWebHostEnvironment webHostEnvironment,
                   IContentLocator contentLocator,
                   IContentVisibility contentVisibility) {
        _umbracoContextFactory = umbracoContextFactory;
        _webHostEnvironment = webHostEnvironment;
        _contentLocator = contentLocator;
        _contentVisibility = contentVisibility;
    }

    public IReadOnlyList<SitemapEntry> GetEntries() {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            var publicContent = _contentLocator.All()
                                               .Where(x => _contentVisibility.IsVisible(x))
                                               .Select(x => new SitemapEntry(x.AbsoluteUrl(),
                                                                             "daily",
                                                                             0.5f,
                                                                             x.UpdateDate.ToLocalDateTime().Date))
                                               .ToList();

            return publicContent;
        }
    }

    public async Task PublishAsync() {
        var sitemapXml = GetXml();

        await WebRoot.SaveTextAsync(_webHostEnvironment, "sitemap.xml", sitemapXml);
    }

    private string GetXml() {
        var sitemap = new XSitemap();
        var entries = GetEntries();

        foreach (var entry in entries) {
            sitemap.Add(new Url {
                ChangeFrequency = ChangeFrequency.Daily,
                Location = entry.Url,
                LastMod = entry.LastModified.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                Priority = 0.5,
                TimeStamp = entry.LastModified.ToDateTimeUnspecified()
            });
        }

        return sitemap.ToXml();
    }
}

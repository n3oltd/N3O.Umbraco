using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Robots;

public class RobotsTxt : IRobotsTxt {
    private const string SitemapFileName = "sitemap.xml";

    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentCache _contentCache;
    private readonly IUrlBuilder _urlBuilder;

    public RobotsTxt(IUmbracoContextFactory umbracoContextFactory,
                     IWebHostEnvironment webHostEnvironment,
                     IContentCache contentCache,
                     IUrlBuilder urlBuilder) {
        _umbracoContextFactory = umbracoContextFactory;
        _webHostEnvironment = webHostEnvironment;
        _contentCache = contentCache;
        _urlBuilder = urlBuilder;
    }

    public async Task PublishAsync() {
        var robotsTxt = GetContent();

        await WebRoot.SaveTextAsync(_webHostEnvironment, "robots.txt", robotsTxt);
    }

    private string GetContent() {
        using (_umbracoContextFactory.EnsureUmbracoContext()) {
            var robotsSettings = _contentCache.Single<RobotsSettingsContent>();

            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");

            if (!_webHostEnvironment.IsProduction()) {
                sb.AppendLine("Disallow: /");
            }

            robotsSettings?.CustomDirectives.IfNotNull(sb.Append);

            var sitemapUrl = GetSitemapUrl();

            if (sitemapUrl.HasValue()) {
                sb.AppendLine();
                sb.AppendLine($"Sitemap: {sitemapUrl}");
            }

            return sb.ToString();
        }
    }

    private string GetSitemapUrl() {
        if (!_webHostEnvironment.IsProduction()) {
            return null;
        }

        try {
            return _urlBuilder.Root().AppendPathSegment(SitemapFileName).ToString();
        } catch {
            return null;
        }
    }
}

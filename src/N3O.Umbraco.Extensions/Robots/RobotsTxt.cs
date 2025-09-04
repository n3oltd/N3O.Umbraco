using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Robots;

public class RobotsTxt : IRobotsTxt {
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentCache _contentCache;

    public RobotsTxt(IUmbracoContextFactory umbracoContextFactory,
                     IWebHostEnvironment webHostEnvironment,
                     IContentCache contentCache) {
        _umbracoContextFactory = umbracoContextFactory;
        _webHostEnvironment = webHostEnvironment;
        _contentCache = contentCache;
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
            sb.AppendLine("Crawl-delay: 10");

            if (!_webHostEnvironment.IsProduction()) {
                sb.AppendLine("Disallow: /");
            }

            robotsSettings?.CustomDirectives.IfNotNull(sb.Append);

            return sb.ToString();
        }
    }
}
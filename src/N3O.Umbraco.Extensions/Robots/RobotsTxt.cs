using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Text;

namespace N3O.Umbraco.Robots;

public class RobotsTxt : IRobotsTxt {
    public static readonly string File = "robots.txt";

    private readonly IContentCache _contentCache;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public RobotsTxt(IContentCache contentCache, IWebHostEnvironment webHostEnvironment) {
        _contentCache = contentCache;
        _webHostEnvironment = webHostEnvironment;
    }

    public string GetContent() {
        var robots = _contentCache.Single<RobotsContent>();

        var sb = new StringBuilder();

        sb.AppendLine("User-agent: *");
        sb.AppendLine("Crawl-delay: 10");

        if (!_webHostEnvironment.IsProduction()) {
            sb.AppendLine("Disallow: /");
        }

        robots.CustomDirectives.IfNotNull(sb.Append);

        return sb.ToString();
    }
}
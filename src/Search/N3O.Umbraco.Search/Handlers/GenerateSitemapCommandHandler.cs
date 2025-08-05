using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using N3O.Umbraco.Search.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Search.Handlers;

[RecurringJob("Regenerate Sitemap", "0 */3 * * *")]
public class GenerateSitemapCommandHandler : IRequestHandler<GenerateSitemapCommand, None, None> {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ISitemap _sitemap;

    public GenerateSitemapCommandHandler(IWebHostEnvironment webHostEnvironment, ISitemap sitemap) {
        _webHostEnvironment = webHostEnvironment;
        _sitemap = sitemap;
    }

    public async Task<None> Handle(GenerateSitemapCommand req, CancellationToken cancellationToken) {
        await _webHostEnvironment.SaveFiletoWwwroot(SearchConstants.SitemapXml, _sitemap.GetXml());
        
        return None.Empty;
    }
}
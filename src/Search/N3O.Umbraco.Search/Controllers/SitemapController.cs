using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Content;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Search.Controllers;

public class SitemapController : UmbracoPageController, IVirtualPageController {
    private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();
    
    private readonly IUmbracoContextFactory _umbracoContextFactory;
    private readonly IPublishedRouter _publishedRouter;
    private readonly ISitemap _sitemap;

    public SitemapController(ILogger<UmbracoPageController> logger,
                             ICompositeViewEngine compositeViewEngine,
                             IUmbracoContextFactory umbracoContextFactory,
                             IPublishedRouter publishedRouter,
                             ISitemap sitemap)
        : base(logger, compositeViewEngine) {
        _umbracoContextFactory = umbracoContextFactory;
        _publishedRouter = publishedRouter;
        _sitemap = sitemap;
    }
    
    [HttpGet]
    public async Task IndexAsync() {
        Response.ContentType = "application/xml";
        await Response.WriteAsync(_sitemap.GetXml());
    }
    
    // public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) {
    //     var contentType = _umbracoContextAccessor.GetContentCache().GetContentType(SitemapAlias);
    //     var content = contentType.IfNotNull(x => _umbracoContextAccessor.GetContentCache().GetByContentType(x))?.SingleOrDefault();
    //
    //     return content;
    // }
    
    // https://github.com/umbraco/Umbraco-CMS/issues/12834#issue-1338670897
    // waiting on https://github.com/umbraco/Umbraco-CMS/pull/15121
    public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) {
        var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
        var contentType = umbracoContext.Content.GetContentType(SitemapAlias);
        var content = contentType.IfNotNull(x => umbracoContext.Content.GetByContentType(x))?.SingleOrDefault();
            
        if (content != null) {
            var requestBuilder = Task.Run(async () => await _publishedRouter.CreateRequestAsync(umbracoContext.CleanedUmbracoUrl)).Result;
            requestBuilder.SetPublishedContent(content);

            umbracoContext.PublishedRequest = requestBuilder.Build();

            return content;
        }

        return null;
    }
}

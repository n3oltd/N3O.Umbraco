using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Search.Content;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Search.Controllers {
    public class SitemapController : UmbracoPageController, IVirtualPageController {
        private static readonly string SitemapAlias = AliasHelper<SitemapContent>.ContentTypeAlias();
        
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly ISitemap _sitemap;

        public SitemapController(ILogger<UmbracoPageController> logger,
                                 ICompositeViewEngine compositeViewEngine,
                                 IUmbracoContextFactory umbracoContextFactory,
                                 ISitemap sitemap)
            : base(logger, compositeViewEngine) {
            _umbracoContextFactory = umbracoContextFactory;
            _sitemap = sitemap;
        }
        
        [HttpGet]
        public async Task IndexAsync() {
            Response.ContentType = "application/xml";
            await Response.WriteAsync(_sitemap.GetXml());
        }
        
        public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) {
            var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            
            var content = umbracoContext.Content
                                        .GetByContentType(umbracoContext.Content.GetContentType(SitemapAlias))
                                        .SingleOrDefault();

            return content;
        }
    }
}
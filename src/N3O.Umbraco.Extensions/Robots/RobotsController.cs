using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Robots {
    public class RobotsController : UmbracoPageController, IVirtualPageController {
        private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();
        
        private readonly IUmbracoContextFactory _umbracoContextFactory;
        private readonly IRobotsTxt _robotsTxt;

        public RobotsController(ILogger<UmbracoPageController> logger,
                                ICompositeViewEngine compositeViewEngine,
                                IUmbracoContextFactory umbracoContextFactory,
                                IRobotsTxt robotsTxt)
            : base(logger, compositeViewEngine) {
            _umbracoContextFactory = umbracoContextFactory;
            _robotsTxt = robotsTxt;
        }
        
        [HttpGet]
        public async Task IndexAsync() {
            Response.ContentType = "text/plain";
            await Response.WriteAsync(_robotsTxt.GetContent());
        }
        
        public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) {
            var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext().UmbracoContext;
            
            var content = umbracoContext.Content
                                        .GetByContentType(umbracoContext.Content.GetContentType(RobotsAlias))
                                        .SingleOrDefault();

            return content;
        }
    }
}
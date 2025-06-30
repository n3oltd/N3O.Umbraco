using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Robots;

public class RobotsController : UmbracoPageController, IVirtualPageController {
    private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();
    
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IRobotsTxt _robotsTxt;

    public RobotsController(ILogger<UmbracoPageController> logger,
                            ICompositeViewEngine compositeViewEngine,
                            IUmbracoContextAccessor umbracoContextAccessor,
                            IRobotsTxt robotsTxt)
        : base(logger, compositeViewEngine) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _robotsTxt = robotsTxt;
    }
    
    [HttpGet]
    public async Task IndexAsync() {
        Response.ContentType = "text/plain";
        await Response.WriteAsync(_robotsTxt.GetContent());
    }
    
    public IPublishedContent FindContent(ActionExecutingContext actionExecutingContext) {
        var contentType = _umbracoContextAccessor.GetContentCache().GetContentType(RobotsAlias);
        var content = contentType.IfNotNull(x => _umbracoContextAccessor.GetContentCache().GetByContentType(x))?.SingleOrDefault();
    
        return content;
    }
}

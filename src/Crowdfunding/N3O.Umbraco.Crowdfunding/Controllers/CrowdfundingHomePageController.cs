using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Crowdfunding.Controllers;

public class CrowdfundingHomePageController : PageController {
    private readonly ICrowdfundingRouter _crowdfundingRouter;

    public CrowdfundingHomePageController(ILogger<RenderController> logger,
                                          ICompositeViewEngine compositeViewEngine,
                                          IUmbracoContextAccessor umbracoContextAccessor,
                                          IPublishedUrlProvider publishedUrlProvider,
                                          IPagePipeline pagePipeline,
                                          IContentCache contentCache,
                                          IServiceProvider serviceProvider,
                                          ICrowdfundingRouter crowdfundingRouter) 
        : base(logger,
               compositeViewEngine,
               umbracoContextAccessor,
               publishedUrlProvider,
               pagePipeline,
               contentCache,
               serviceProvider) {
        _crowdfundingRouter = crowdfundingRouter;
    }

    public override async Task<IActionResult> Index(CancellationToken cancellationToken) {
        if (_crowdfundingRouter.CurrentPage == null) {
            return Redirect(SpecialPages.NotFound);
        } else if (_crowdfundingRouter.CurrentPage.NoCache) {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Expires"] = "-1";
            Response.Headers["Pragma"] = "no-cache";
        }

        return await base.Index(cancellationToken);
    }
}
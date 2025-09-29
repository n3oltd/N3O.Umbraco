using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Pages;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Giving.Checkout.Controllers;

public class CheckoutAccountPageController : CheckoutStagePageController {
    public CheckoutAccountPageController(ILogger<CheckoutAccountPageController> logger,
                                         ICompositeViewEngine compositeViewEngine,
                                         IUmbracoContextAccessor umbracoContextAccessor,
                                         IPublishedUrlProvider publishedUrlProvider,
                                         IPagePipeline pagePipeline,
                                         IContentCache contentCache,
                                         IServiceProvider serviceProvider,
                                         ICheckoutAccessor checkoutAccessor,
                                         IEnumerable<IContentRenderabilityFilter> contentRenderabilityFilters)
        : base(logger,
               compositeViewEngine,
               umbracoContextAccessor,
               publishedUrlProvider,
               pagePipeline,
               contentCache,
               serviceProvider,
               checkoutAccessor,
               contentRenderabilityFilters) { }

    protected override CheckoutStage Stage => CheckoutStages.Account;
}

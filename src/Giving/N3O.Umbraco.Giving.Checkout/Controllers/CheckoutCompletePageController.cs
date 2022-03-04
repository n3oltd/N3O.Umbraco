using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Pages;
using System;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    public class CheckoutCompletePageController : CheckoutStagePageController {
        public CheckoutCompletePageController(ILogger<CheckoutCompletePageController> logger,
                                              ICompositeViewEngine compositeViewEngine,
                                              IUmbracoContextAccessor umbracoContextAccessor,
                                              IPublishedUrlProvider publishedUrlProvider,
                                              IPagePipeline pagePipeline,
                                              IContentCache contentCache,
                                              IServiceProvider serviceProvider,
                                              ICheckoutAccessor checkoutAccessor)
            : base(logger,
                   compositeViewEngine,
                   umbracoContextAccessor,
                   publishedUrlProvider,
                   pagePipeline,
                   contentCache,
                   serviceProvider,
                   checkoutAccessor) { }

        protected override CheckoutStage Stage => null;
    }
}
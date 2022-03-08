using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Cart.Context;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    public class CheckoutCompletePageController : CheckoutStagePageController {
        private readonly CartCookie _cartCookie;

        public CheckoutCompletePageController(ILogger<CheckoutCompletePageController> logger,
                                              ICompositeViewEngine compositeViewEngine,
                                              IUmbracoContextAccessor umbracoContextAccessor,
                                              IPublishedUrlProvider publishedUrlProvider,
                                              IPagePipeline pagePipeline,
                                              IContentCache contentCache,
                                              IServiceProvider serviceProvider,
                                              ICheckoutAccessor checkoutAccessor,
                                              CartCookie cartCookie)
            : base(logger,
                   compositeViewEngine,
                   umbracoContextAccessor,
                   publishedUrlProvider,
                   pagePipeline,
                   contentCache,
                   serviceProvider,
                   checkoutAccessor) {
            _cartCookie = cartCookie;
        }

        protected override CheckoutStage Stage => null;

        public override async Task<IActionResult> Index(CancellationToken cancellationToken) {
            _cartCookie.DeferredReset();
            
            return await base.Index(cancellationToken);
        }
    }
}
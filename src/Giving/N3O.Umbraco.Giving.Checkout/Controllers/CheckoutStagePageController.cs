using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Checkout.Content;
using N3O.Umbraco.Giving.Content;
using System.Threading;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using N3O.Umbraco.Giving.Checkout;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Pages;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    public abstract class CheckoutStagePageController : PageController {
        private readonly ICheckoutAccessor _checkoutAccessor;
        private readonly IContentCache _contentCache;

        protected CheckoutStagePageController(ILogger<RenderController> logger,
                                              ICompositeViewEngine compositeViewEngine,
                                              IUmbracoContextAccessor umbracoContextAccessor,
                                              IPublishedUrlProvider publishedUrlProvider,
                                              IPagePipeline pagePipeline,
                                              IContentCache contentCache,
                                              IServiceProvider serviceProvider,
                                              ICheckoutAccessor checkoutAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor, publishedUrlProvider, pagePipeline, contentCache, serviceProvider) {
            _checkoutAccessor = checkoutAccessor;
            _contentCache = contentCache;
        }

        public override async Task<IActionResult> Index(CancellationToken cancellationToken) {
            var checkout = _checkoutAccessor.GetOrCreateAsync(CancellationToken.None).GetAwaiter().GetResult();
            string redirectUrl = null;

            if (checkout == null) {
                redirectUrl = _contentCache.Single<DonatePageContent>().Content().AbsoluteUrl();
            } else if (checkout.IsComplete) {
                redirectUrl = _contentCache.Single<CheckoutCompletePageContent>().Content().AbsoluteUrl();
            } else if (checkout.Progress.CurrentStage != Stage) {
                redirectUrl = checkout.Progress.CurrentStage.GetUrl(_contentCache);
            }

            if (redirectUrl.HasValue()) {
                return Redirect(redirectUrl);
            } else {
                return await base.Index(cancellationToken);
            }
        }

        protected abstract CheckoutStage Stage { get; }
    }
}
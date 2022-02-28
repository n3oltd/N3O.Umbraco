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

namespace N3O.Umbraco.Giving.Checkout.Controllers {
    public class CheckoutPageController : RenderController {
        private readonly ICheckoutAccessor _checkoutAccessor;
        private readonly IQueryStringAccessor _queryStringAccessor;
        private readonly IContentCache _contentCache;

        public CheckoutPageController(ILogger<CheckoutPageController> logger,
                                      ICompositeViewEngine compositeViewEngine,
                                      IUmbracoContextAccessor umbracoContextAccessor,
                                      ICheckoutAccessor checkoutAccessor,
                                      IQueryStringAccessor queryStringAccessor,
                                      IContentCache contentCache)
            : base(logger, compositeViewEngine, umbracoContextAccessor) {
            _checkoutAccessor = checkoutAccessor;
            _queryStringAccessor = queryStringAccessor;
            _contentCache = contentCache;
        }

        public override IActionResult Index() {
            if (_queryStringAccessor.Has("framed")) {
                return CurrentTemplate(new ContentModel(CurrentPage));
            } else {
                var checkout = _checkoutAccessor.GetOrCreateAsync(CancellationToken.None).GetAwaiter().GetResult();
                string url;

                if (checkout == null) {
                    url = _contentCache.Single<DonatePageContent>().Content.AbsoluteUrl();
                } else if (checkout.IsComplete) {
                    url = _contentCache.Single<CheckoutCompletePageContent>().Content.AbsoluteUrl();
                } else {
                    url = checkout.Progress.CurrentStage.GetUrl(_contentCache);
                }

                return Redirect(url) ;
            }
        }
    }
}
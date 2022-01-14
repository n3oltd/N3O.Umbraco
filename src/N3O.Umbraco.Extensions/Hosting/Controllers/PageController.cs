using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Website.ActionResults;

namespace N3O.Umbraco.Hosting {
    public class PageController : RenderController {
        private readonly IPublishedUrlProvider _publishedUrlProvider;
        private readonly IPagePipeline _pagePipeline;
        private readonly IContentCache _contentCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public PageController(ILogger<RenderController> logger,
                              ICompositeViewEngine compositeViewEngine,
                              IUmbracoContextAccessor umbracoContextAccessor,
                              IPublishedUrlProvider publishedUrlProvider,
                              IPagePipeline pagePipeline,
                              IContentCache contentCache,
                              IServiceProvider serviceProvider)
            : base(logger, compositeViewEngine, umbracoContextAccessor) {
            _umbracoContextAccessor = umbracoContextAccessor;
            _publishedUrlProvider = publishedUrlProvider;
            _pagePipeline = pagePipeline;
            _contentCache = contentCache;
            _serviceProvider = serviceProvider;
        }

        [NonAction]
        public sealed override IActionResult Index() => throw new NotImplementedException();

        public virtual async Task<IActionResult> Index(CancellationToken cancellationToken) {
            var viewModel = await GetViewModelAsync(cancellationToken);

            return CurrentTemplate(viewModel);
        }

        protected IActionResult Redirect<T>() where T : IPublishedContent {
            var content = _contentCache.Single<T>();

            return new RedirectToUmbracoPageResult(content, _publishedUrlProvider, _umbracoContextAccessor);
        }
    
        protected async Task<object> GetViewModelAsync(CancellationToken cancellationToken = default) {
            var pageModuleData = await _pagePipeline.RunAsync(CurrentPage, cancellationToken);
            var factoryType = typeof(IPageViewModelFactory<>).MakeGenericType(CurrentPage.GetType());
            var factory = (IPageViewModelFactory) _serviceProvider.GetService(factoryType);
            var viewModel = factory.Create(CurrentPage, pageModuleData);

            return viewModel;
        }
    }
}

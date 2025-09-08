using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Website.ActionResults;

namespace N3O.Umbraco.Hosting;

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

    protected IActionResult Redirect(SpecialContent specialContent) {
        var publishedContent = _contentCache.Special(specialContent);

        return Redirect(publishedContent);
    }
    
    protected IActionResult Redirect<T>() {
        var content = _contentCache.Single<T>();

        if (content is IPublishedContent publishedContent) {
            return Redirect(publishedContent);
        } else if (content is UmbracoContent<T> umbracoContent) {
            return Redirect(umbracoContent.Content());
        } else {
            throw new Exception($"{typeof(T)} must be {nameof(IPublishedContent)} or {nameof(UmbracoContent<T>)}");
        }
    }

    protected async Task<IPageViewModel> GetViewModelAsync(CancellationToken cancellationToken = default) {
        var pageType = CurrentPage.GetType();
        var pageModuleData = await _pagePipeline.RunAsync(CurrentPage, cancellationToken);
        var factoryType = typeof(IPageViewModelFactory<>).MakeGenericType(pageType);
        var factory = (IPageViewModelFactory) _serviceProvider.GetService(factoryType);

        if (factory == null) {
            factory = PageViewModelFactory.Default(_serviceProvider, pageType);
        }
        
        var viewModel = factory.Create(CurrentPage, pageModuleData);

        return viewModel;
    }
    
    private IActionResult Redirect(IPublishedContent publishedContent) {
        return new RedirectToUmbracoPageResult(publishedContent, _publishedUrlProvider, _umbracoContextAccessor);
    }
}

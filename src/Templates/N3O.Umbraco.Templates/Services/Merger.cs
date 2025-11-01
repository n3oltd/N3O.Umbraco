using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Templates.Extensions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Routing;

namespace N3O.Umbraco.Templates;

public class Merger : IMerger {
    private readonly ConcurrentDictionary<IPublishedContent, IReadOnlyDictionary<string, object>> _mergeModelsCache = new();
    private readonly IHtmlHelper _htmlHelper;
    private readonly ITemplateEngine _templateEngine;
    private readonly IEnumerable<IMergeModelProvider> _mergeModelProviders;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Merger(IHtmlHelper htmlHelper,
                  ITemplateEngine templateEngine,
                  IEnumerable<IMergeModelProvider> mergeModelProviders,
                  IHttpContextAccessor httpContextAccessor) {
        _htmlHelper = htmlHelper;
        _templateEngine = templateEngine;
        _mergeModelProviders = mergeModelProviders;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<string> MergeForAsync(IPublishedContent content,
                                            string markup,
                                            CancellationToken cancellationToken = default) {
        var mergeModels = await _mergeModelProviders.GetMergeModelsAsync(content, _mergeModelsCache);
        var html = _templateEngine.Render(markup, mergeModels);

        return html;
    }

    public async Task<IHtmlContent> MergePartialForAsync(IPublishedContent content,
                                                         ViewContext viewContext,
                                                         string partialViewName,
                                                         object model,
                                                         CancellationToken cancellationToken = default) {
        var mergeModels = new Dictionary<string, object>();

        if (content.HasValue()) {
            mergeModels = (await _mergeModelProviders.GetMergeModelsAsync(content, _mergeModelsCache)).ToDictionary();
        }
        
        (_htmlHelper as IViewContextAware)?.Contextualize(viewContext);
        
        var htmlContent = await _htmlHelper.PartialAsync(partialViewName, model);

        return new MergedHtmlContent(_templateEngine, htmlContent, mergeModels);
    }

    public async Task<IHtmlContent> MergePartialForCurrentContentAsync(ViewContext  viewContext,
                                                                       string partialViewName,
                                                                       object model,
                                                                       CancellationToken cancellationToken = default) {
        var currentContent = GetCurrentContent();
        
        return await MergePartialForAsync(currentContent, viewContext, partialViewName, model, cancellationToken);
    }
    
    private IPublishedContent GetCurrentContent() {
        var umbracoRouteValues = _httpContextAccessor.HttpContext.Features.Get<UmbracoRouteValues>();

        return umbracoRouteValues?.PublishedRequest.PublishedContent;
    }
}
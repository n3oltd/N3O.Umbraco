using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.Extensions;
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
    private readonly IReadOnlyList<IMergeModelProvider> _mergeModelProviders;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Merger(IHtmlHelper htmlHelper,
                  ITemplateEngine templateEngine,
                  IEnumerable<IMergeModelProvider> mergeModelProviders,
                  IHttpContextAccessor httpContextAccessor) {
        _htmlHelper = htmlHelper;
        _templateEngine = templateEngine;
        _mergeModelProviders = mergeModelProviders.ApplyAttributeOrdering();
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<string> MergeForAsync(IPublishedContent content,
                                            string markup,
                                            CancellationToken cancellationToken = default) {
        var mergeModels = await GetMergeModelsAsync(content, cancellationToken);
        var html = _templateEngine.Render(markup, mergeModels);

        return html;
    }

    public async Task<IHtmlContent> MergePartialForAsync(IPublishedContent content,
                                                         string partialViewName,
                                                         object model,
                                                         CancellationToken cancellationToken = default) {
        var mergeModels = await GetMergeModelsAsync(content, cancellationToken);
        var htmlContent = await _htmlHelper.PartialAsync(partialViewName, model);

        return new MergerHtmlContent(_templateEngine, htmlContent, mergeModels);
    }

    public async Task<IHtmlContent> MergePartialForCurrentContentAsync(string partialViewName,
                                                                       object model,
                                                                       CancellationToken cancellationToken = default) {
        var currentContent = GetCurrentContent();
        
        return await MergePartialForAsync(currentContent, partialViewName, model, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<string, object>> GetMergeModelsAsync(IPublishedContent content,
                                                                                CancellationToken cancellationToken) {
        return await _mergeModelsCache.GetOrAddAtomicAsync<IPublishedContent, IReadOnlyDictionary<string, object>>(content, async () => {
            var mergeModels = new Dictionary<string, object>();

            foreach (var provider in _mergeModelProviders) {
                if (await provider.IsProviderForAsync(content)) {
                    mergeModels[provider.Key.Camelize()] = await provider.GetAsync(content, cancellationToken);   
                }
            }

            return mergeModels;
        });
    }
    
    private IPublishedContent GetCurrentContent() {
        var umbracoRouteValues = _httpContextAccessor.HttpContext.Features.Get<UmbracoRouteValues>();

        return umbracoRouteValues.PublishedRequest.PublishedContent;
    }
}
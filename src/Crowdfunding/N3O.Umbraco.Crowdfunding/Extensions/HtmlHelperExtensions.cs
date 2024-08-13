using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.CrowdFunding.Extensions;

public static class HtmlHelperExtensions {
    // TODO Talha these methods provide your hook to look for a per client override of either the page template or a
    // component template and use that instead. Will probably want to cache the search so not looking on disk for every
    // request.
    public static async Task<IHtmlContent> RenderCrowdfundingPageAsync(this IHtmlHelper html,
                                                                       ICrowdfundingPage crowdfundingPage,
                                                                       Uri requestUri) {
        var viewModel = await crowdfundingPage.GetViewModelAsync(requestUri);
        
        var viewFile = $"~/Views/Partials/Crowdfunding/{crowdfundingPage.ViewName}.cshtml";

        return await html.PartialAsync(viewFile, viewModel);
    }
    
    public static async Task<IHtmlContent> RenderCrowdfundingComponentAsync(this IHtmlHelper html,
                                                                            string partialView,
                                                                            object viewModel) {
        var viewFile = $"~/Views/Partials/Crowdfunding/Components/{partialView}.cshtml";

        return await html.PartialAsync(viewFile, viewModel);
    }
}
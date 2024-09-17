using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class HtmlHelperExtensions {
    public static IHtmlContent CrowdfundingPartial(this IHtmlHelper htmlHelper,
                                                   string partialViewName,
                                                   object viewModel) {
        return htmlHelper.Partial(partialViewName, viewModel);
    }
}
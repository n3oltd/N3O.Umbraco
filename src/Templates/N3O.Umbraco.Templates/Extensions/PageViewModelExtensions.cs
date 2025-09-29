using N3O.Umbraco.Pages;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates.Extensions;

public static class PageViewModelExtensions {
    public static IReadOnlyDictionary<string, object> MergeModels(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<IReadOnlyDictionary<string, object>>(TemplateConstants.PageModuleKeys.MergeModels);
    }
}

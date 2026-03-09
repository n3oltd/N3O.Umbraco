using N3O.Umbraco.Pages;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates.Extensions;

public static class PageViewModelExtensions {
    public static object MergeModel(this IPageViewModel pageViewModel, string key) {
        var mergeModel = MergeModels(pageViewModel);

        return mergeModel.GetValueOrDefault(key);
    }
    
    public static T MergeModel<T>(this IPageViewModel pageViewModel, string key) {
        return (T) MergeModel(pageViewModel, key);
    }
    
    public static IReadOnlyDictionary<string, object> MergeModels(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<IReadOnlyDictionary<string, object>>(TemplateConstants.PageModuleKeys.MergeModels);
    }
}

using N3O.Umbraco.Json;
using N3O.Umbraco.Pages;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Templates.Extensions;

public static class PageViewModelExtensions {
    public static object MergeModel(this IPageViewModel pageViewModel, string key) {
        var mergeModel = MergeModels(pageViewModel);

        return mergeModel.GetValueOrDefault(key);
    }
    
    public static T MergeModel<T>(this IPageViewModel pageViewModel,
                                  IJsonProvider jsonProvider,
                                  string key) {
        var model = MergeModel(pageViewModel, key);

        if (model is T typedModel) {
            return  typedModel;
        }
        
        return jsonProvider.DeserializeDynamicTo<T>(model);
    }
    
    public static IReadOnlyDictionary<string, object> MergeModels(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<IReadOnlyDictionary<string, object>>(TemplateConstants.PageModuleKeys.MergeModels);
    }
}

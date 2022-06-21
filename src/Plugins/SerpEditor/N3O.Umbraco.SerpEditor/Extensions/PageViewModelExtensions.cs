using N3O.Umbraco.Pages;
using N3O.Umbraco.SerpEditor.Models;

namespace N3O.Umbraco.SerpEditor.Extensions;

public static class PageViewModelExtensions {
    public static SerpEntry SerpEntry(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<SerpEntry>(SerpEditorConstants.PageModuleKeys.SerpEntry);
    }
}

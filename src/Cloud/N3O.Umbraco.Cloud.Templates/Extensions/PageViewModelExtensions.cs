using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Json;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Templates.Extensions;

namespace N3O.Umbraco.Cloud.Templates.Extensions;

public static class PageViewModelExtensions {
    public static IOrganizationInfo OrganizationInfo(this IPageViewModel pageViewModel, IJsonProvider jsonProvider) {
        return pageViewModel.MergeModel<IOrganizationInfo>(jsonProvider,
                                                           CloudTemplatesConstants.ModelKeys.OrganizationInfo);
    }
}

using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Templates.Extensions;

namespace N3O.Umbraco.Cloud.Templates.Extensions;

public static class PageViewModelExtensions {
    public static IOrganizationInfo OrganizationInfo(this IPageViewModel pageViewModel) {
        return pageViewModel.MergeModel<IOrganizationInfo>(CloudTemplatesConstants.ModelKeys.OrganizationInfo);
    }
}

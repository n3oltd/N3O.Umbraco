using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using UmbracoSecurity = Umbraco.Cms.Core.Constants.Security;

namespace N3O.Umbraco.Data.Controllers;

public class ImportVisibilityController : WorkspaceVisibilityController {
    private readonly IEnumerable<IImportContentFilter> _contentFilters;
    private readonly IContentService _contentService;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

    public ImportVisibilityController(IEnumerable<IImportContentFilter> contentFilters,
                                      IContentService contentService,
                                      IBackOfficeSecurityAccessor backOfficeSecurityAccessor) {
        _contentFilters = contentFilters.OrEmpty().ToList();
        _contentService = contentService;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    protected override Task<bool> IsVisibleAsync(Guid contentId) {
        return Task.FromResult(IsPermitted(contentId));
    }

    private bool IsPermitted(Guid contentId) {
        var userGroups = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.Groups.OrEmpty().ToList();

        if (userGroups == null) {
            return false;
        }

        if (userGroups.All(x => !x.Alias.EqualsInvariant(UmbracoSecurity.AdminGroupAlias) &&
                                !x.Alias.EqualsInvariant(DataConstants.SecurityGroups.ImportUsers.Alias))) {
            return false;
        }

        var content = _contentService.GetById(contentId);

        if (content == null || content.Id == 0) {
            return false;
        }

        var filter = _contentFilters.SingleOrDefault(x => x.IsFilter(content));

        return filter?.AllowImports(content) == true;
    }
}

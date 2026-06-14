using Microsoft.AspNetCore.Mvc;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using UmbracoSecurity = Umbraco.Cms.Core.Constants.Security;

namespace N3O.Umbraco.Data.Controllers;

// Restores the server-side visibility gating the AngularJS ImportApp content app used to enforce via
// IContentAppFactory.GetContentAppFor before the Bellissima migration. The Import workspace view calls
// this endpoint (via the N3O.Condition.WorkspaceVisibility frontend condition) to decide whether to show.
// Reachable by any authenticated back-office user so non-import users receive { permitted: false } rather
// than a 403; the group/filter gating is done in code below.
[Route("/umbraco/backoffice/api/Imports")]
public class ImportVisibilityController : BackofficeAuthorizedApiController {
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

    [HttpGet("visibility/{contentId:guid}")]
    public IActionResult GetVisibility([FromRoute] Guid contentId) {
        return Ok(new { permitted = IsPermitted(contentId) });
    }

    private bool IsPermitted(Guid contentId) {
        var userGroups = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.Groups.OrEmpty().ToList();

        if (userGroups == null) {
            return false;
        }

        // User must be an Umbraco admin OR a member of the Import Users group
        if (userGroups.All(x => !x.Alias.EqualsInvariant(UmbracoSecurity.AdminGroupAlias) &&
                                x.Name != DataConstants.SecurityGroups.ImportUsers.Name)) {
            return false;
        }

        var content = _contentService.GetById(contentId);

        if (content == null || content.Id == default) {
            return false;
        }

        var filter = _contentFilters.SingleOrDefault(x => x.IsFilter(content));

        return filter?.AllowImports(content) == true;
    }
}

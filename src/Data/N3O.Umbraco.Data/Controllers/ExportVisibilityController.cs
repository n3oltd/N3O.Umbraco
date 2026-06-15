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

// Restores the server-side visibility gating the AngularJS ExportApp content app used to enforce via
// IContentAppFactory.GetContentAppFor before the Bellissima migration. The Export workspace view calls
// this endpoint (via the N3O.Condition.WorkspaceVisibility frontend condition) to decide whether to show.
// Reachable by any authenticated back-office user so non-export users receive { permitted: false } rather
// than a 403; the group/filter gating is done in code below.
// No explicit [Route] — inherits the base /umbraco/backoffice/api/[controller] (=> .../ExportVisibility).
public class ExportVisibilityController : BackofficeAuthorizedApiController {
    private readonly IEnumerable<IExportContentFilter> _contentFilters;
    private readonly IContentService _contentService;
    private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

    public ExportVisibilityController(IEnumerable<IExportContentFilter> contentFilters,
                                      IContentService contentService,
                                      IBackOfficeSecurityAccessor backOfficeSecurityAccessor) {
        _contentFilters = contentFilters.OrEmpty().ToList();
        _contentService = contentService;
        _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
    }

    [HttpGet("{contentId:guid}")]
    public IActionResult GetVisibility([FromRoute] Guid contentId) {
        return Ok(new { permitted = IsPermitted(contentId) });
    }

    private bool IsPermitted(Guid contentId) {
        var userGroups = _backOfficeSecurityAccessor.BackOfficeSecurity?.CurrentUser?.Groups.OrEmpty().ToList();

        if (userGroups == null) {
            return false;
        }

        if (userGroups.All(x => !x.Alias.EqualsInvariant(UmbracoSecurity.AdminGroupAlias) &&
                                !x.Alias.EqualsInvariant(DataConstants.SecurityGroups.ExportUsers.Alias))) {
            return false;
        }

        var content = _contentService.GetById(contentId);

        if (content == null || content.Id == 0) {
            return false;
        }

        var filter = _contentFilters.SingleOrDefault(x => x.IsFilter(content));

        return filter?.AllowExports(content) == true;
    }
}

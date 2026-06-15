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

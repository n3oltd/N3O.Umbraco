using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Controllers;

public class PlatformsPreviewVisibilityController : WorkspaceVisibilityController {
    private static readonly string[] CompositionAliases = [PlatformsConstants.Offerings.CompositionAlias];

    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;

    public PlatformsPreviewVisibilityController(IContentService contentService, IContentTypeService contentTypeService) {
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }

    protected override Task<bool> IsVisibleAsync(Guid contentId) {
        return Task.FromResult(IsPermitted(contentId));
    }

    private bool IsPermitted(Guid contentId) {
        var content = _contentService.GetById(contentId);

        if (content == null) {
            return false;
        }

        var contentType = _contentTypeService.Get(content.ContentTypeId);

        if (contentType == null) {
            return false;
        }

        return contentType.CompositionAliases().ContainsAny(CompositionAliases, true);
    }
}

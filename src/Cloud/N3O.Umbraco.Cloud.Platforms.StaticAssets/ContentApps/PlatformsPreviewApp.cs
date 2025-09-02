using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsPreviewApp : IContentAppFactory {
    private readonly IContentTypeService _contentTypeService;

    public PlatformsPreviewApp(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }
    
    public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups) {
        var content = source as IContent;

        if (content == null) {
            return null;
        }

        var contentType = _contentTypeService.Get(content.ContentTypeId);
        var compositionAliases = contentType.CompositionAliases();
        
        if (!compositionAliases.ContainsAny([PlatformsConstants.Designations.CompositionAlias, PlatformsConstants.Elements.CompositionAlias], true)) {
            return null;
        }

        return new ContentApp {
            Alias = "platformsPreview",
            Name = "Preview",
            Icon = "icon-eye",
            View = "/App_Plugins/N3O.Umbraco.Cloud.Platforms.Preview/N3O.Umbraco.Cloud.Platforms.Preview.html",
            Weight = -999
        };
    }
}
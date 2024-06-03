using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public class PageModeAccessor : IPageModeAccessor {
    private readonly IEnumerable<IContentAccessControl> _contentAccessControls;

    public PageModeAccessor(IEnumerable<IContentAccessControl> contentAccessControls) {
        _contentAccessControls = contentAccessControls;
    }
    
    public PageMode GetPageMode(IPublishedContent content) {
        foreach (var contentAccessControl in _contentAccessControls) {
            if (contentAccessControl.CanEditAsync(content).GetAwaiter().GetResult() == false) {
                return PageModes.View;
            }
        }
        
        return PageModes.Edit;
    }
}
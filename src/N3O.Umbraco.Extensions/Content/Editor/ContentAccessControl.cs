using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public abstract class ContentAccessControl : IContentAccessControl {
    private readonly IContentHelper _contentHelper;
    private readonly IVariationContextAccessor _variationContextAccessor;

    protected ContentAccessControl(IContentHelper contentHelper, IVariationContextAccessor variationContextAccessor) {
        _contentHelper = contentHelper;
        _variationContextAccessor = variationContextAccessor;
    }
    
    public async Task<bool> CanEditAsync(IContent content) {
        if (!IsAccessControlFor(content.ContentType.Alias)) {
            return true;
        }

        var contentProperties = _contentHelper.GetContentProperties(content, _variationContextAccessor?.VariationContext?.Culture);

        return await AllowEditAsync(contentProperties);
    }

    public async Task<bool> CanEditAsync(IPublishedContent content) {
        if (!IsAccessControlFor(content.ContentType.Alias)) {
            return true;
        }

        return await AllowEditAsync(content);
    }

    protected abstract Task<bool> AllowEditAsync(ContentProperties contentProperties);
    protected abstract Task<bool> AllowEditAsync(IPublishedContent content);
    protected abstract bool IsAccessControlFor(string contentTypeAlias);
}
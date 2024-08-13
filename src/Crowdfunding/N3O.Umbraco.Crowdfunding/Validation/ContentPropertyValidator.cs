using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.CrowdFunding;

public abstract class ContentPropertyValidator<T> : IContentPropertyValidator where T : ValueReq {
    private readonly string _contentTypeAlias;
    private readonly string _propertyAlias;

    protected ContentPropertyValidator(string contentTypeAlias, string propertyAlias) {
        _contentTypeAlias = contentTypeAlias;
        _propertyAlias = propertyAlias;
    }
    
    public bool IsValidator(string contentTypeAlias, string propertyAlias) {
        return contentTypeAlias.EqualsInvariant(_contentTypeAlias) && propertyAlias.EqualsInvariant(_propertyAlias);
    }

    public bool IsValid(IPublishedContent content, string propertyAlias, ValueReq req) {
        return IsValid(content, propertyAlias, (T) req);
    }

    protected abstract bool IsValid(IPublishedContent content, string propertyAlias, T req);
}
using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class TemplatePublisher<T> : ITemplatePublisher where T : IPublishedContent {
    public bool IsPublisherFor(string alias) {
        return alias == ContentTypeAlias;
    }

    public IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent) {
        return GetContentProperties((T) publishedContent);
    }
    
    protected abstract string ContentTypeAlias { get; }
    protected abstract IEnumerable<PropertyContentReq> GetContentProperties(T publishedContent);
}
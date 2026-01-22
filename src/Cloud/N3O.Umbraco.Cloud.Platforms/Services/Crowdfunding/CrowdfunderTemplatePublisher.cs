using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class CrowdfunderTemplatePublisher<T> : ICrowdfunderTemplatePublisher where T : IPublishedContent {
    public IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent) {
        return GetContentProperties((T) publishedContent);
    }

    protected abstract IEnumerable<PropertyContentReq> GetContentProperties(T publishedContent);
}
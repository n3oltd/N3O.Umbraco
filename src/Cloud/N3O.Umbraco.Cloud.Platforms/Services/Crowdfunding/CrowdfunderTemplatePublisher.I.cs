using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public interface ICrowdfunderTemplatePublisher {
    IEnumerable<PropertyContentReq> GetContentProperties(IPublishedContent publishedContent);
}
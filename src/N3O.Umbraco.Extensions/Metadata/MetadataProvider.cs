using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Metadata;

public abstract class MetadataProvider<T> : IMetadataProvider where T : IPublishedContent {
    public abstract Task<IEnumerable<MetadataEntry>> GetEntriesAsync(IPublishedContent page);

    public virtual bool IsProviderFor(IPublishedContent page) {
        return page.GetType().IsAssignableTo(typeof(T));
    }
}

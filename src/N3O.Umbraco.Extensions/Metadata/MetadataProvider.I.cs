using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Metadata;

public interface IMetadataProvider {
    Task<IEnumerable<MetadataEntry>> GetEntriesAsync(IPublishedContent page);
    bool IsProviderFor(IPublishedContent page);
}

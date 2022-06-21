using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public interface IContentMetadataConverter {
    bool IsConverter(ContentMetadata contentMetadata);
    IColumnRange GetColumnRange(int order);
    object GetValue(IContent content);
}

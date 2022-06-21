using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public abstract class ContentMetadataConverter<TValue> : IContentMetadataConverter {
    private readonly IColumnRangeBuilder _columnRangeBuilder;
    private readonly ContentMetadata _contentMetadata;

    protected ContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder, ContentMetadata contentMetadata) {
        _columnRangeBuilder = columnRangeBuilder;
        _contentMetadata = contentMetadata;
    }
    
    public bool IsConverter(ContentMetadata contentMetadata) {
        return _contentMetadata == contentMetadata;
    }

    public IColumnRange GetColumnRange(int order) {
        return _columnRangeBuilder.OfType<TValue>(_contentMetadata.DataType)
                                  .Title($"{nameof(ContentMetadataConverter<object>)}Title", Title)
                                  .SetOrder(order)
                                  .Build();
    }
    
    public abstract object GetValue(IContent content);

    protected abstract string Title { get; }
}

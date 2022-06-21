using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public class HasUnpublishedChangesContentMetadataConverter : ContentMetadataConverter<bool?> {
    public HasUnpublishedChangesContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder)
        : base(columnRangeBuilder, ContentMetadatas.HasUnpublishedChanges) { }

    public override object GetValue(IContent content) {
        return content.PublishedVersionId == content.VersionId;
    }
    
    protected override string Title => "Has Unpublished Changes";
}

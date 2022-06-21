using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public class IsPublishedContentMetadataConverter : ContentMetadataConverter<bool?> {
    public IsPublishedContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder)
        : base(columnRangeBuilder, ContentMetadatas.IsPublished) { }

    public override object GetValue(IContent content) {
        return content.Published;
    }
    
    protected override string Title => "Is Published";
}

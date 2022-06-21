using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public class NameContentMetadataConverter : ContentMetadataConverter<string> {
    public NameContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder)
        : base(columnRangeBuilder, ContentMetadatas.Name) { }

    public override object GetValue(IContent content) {
        return content.Name;
    }
    
    protected override string Title => "Name";
}

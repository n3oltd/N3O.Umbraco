using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using NodaTime;
using NodaTime.Extensions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters;

public class CreatedAtContentMetadataConverter : ContentMetadataConverter<LocalDateTime?> {
    public CreatedAtContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder)
        : base(columnRangeBuilder, ContentMetadatas.CreatedAt) { }

    public override object GetValue(IContent content) {
        return content.CreateDate.ToLocalDateTime();
    }
    
    protected override string Title => "Created At";
}

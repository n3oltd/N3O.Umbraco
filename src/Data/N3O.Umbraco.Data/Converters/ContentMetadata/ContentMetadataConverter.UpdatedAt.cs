using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using NodaTime;
using NodaTime.Extensions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Converters {
    public class UpdatedAtContentMetadataConverter : ContentMetadataConverter<LocalDateTime?> {
        public UpdatedAtContentMetadataConverter(IColumnRangeBuilder columnRangeBuilder)
            : base(columnRangeBuilder, ContentMetadatas.UpdatedAt) { }

        public override object GetValue(IContent content) {
            return content.UpdateDate.ToLocalDateTime();
        }
        
        protected override string Title => "Updated At";
    }
}
using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models {
    public class ExcelColumn : Column {
        public ExcelColumn(Column column, AggregationFunction footerFunction)
            : base(column.DataType,
                   column.Title,
                   column.Comment,
                   column.Formatter,
                   column.LocalClock,
                   column.LocalizationSettings,
                   column.Hidden,
                   column.AccessControlList,
                   column.Attributes,
                   column.Metadata) {
            FooterFunction = footerFunction;
        }

        public AggregationFunction FooterFunction { get; }
    }
}
using N3O.Umbraco.Data.Attributes;
using N3O.Umbraco.Data.Lookups;
using System.Linq;

namespace N3O.Umbraco.Data.Models {
    public class ExcelFormatting : Value {
        public static ExcelFormatting None = new();

        public ExcelFormatting() {
            FontWeight = FontWeight.Normal;
            HorizontalAlignment = HorizontalAlignment.Left;
        }

        public ExcelFormatting Bold() {
            FontWeight = FontWeight.Bold;

            return this;
        }

        public ExcelFormatting Centre() {
            HorizontalAlignment = HorizontalAlignment.Centre;

            return this;
        }

        public ExcelFormatting LeftAlign() {
            HorizontalAlignment = HorizontalAlignment.Left;

            return this;
        }

        public ExcelFormatting RightAlign() {
            HorizontalAlignment = HorizontalAlignment.Right;

            return this;
        }

        public ExcelFormatting Format(ExcelNumberFormat numberFormat) {
            NumberFormat = numberFormat;

            return this;
        }
        
        public void ApplyFormattingAttributes(Column column) {
            var formattingAttributes = column.Attributes.OfType<FormattingAttribute>().ToList();

            foreach (var attribute in formattingAttributes) {
                attribute.ApplyFormatting(this);
            }
        }

        public FontWeight FontWeight { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public ExcelNumberFormat NumberFormat { get; set; }
    }
}
using N3O.Umbraco.Data.Models;
using System;

namespace N3O.Umbraco.Data.Attributes {
    public class NumberFormatAttribute : FormattingAttribute {
        public NumberFormatAttribute(Type numberFormatType) {
            NumberFormatType = numberFormatType;
        }

        public override void ApplyFormatting(ExcelFormatting formatting) {
            var numberFormat = (ExcelNumberFormat) Activator.CreateInstance(NumberFormatType);

            formatting.NumberFormat = numberFormat;
        }
        
        public Type NumberFormatType { get; }
    }
}
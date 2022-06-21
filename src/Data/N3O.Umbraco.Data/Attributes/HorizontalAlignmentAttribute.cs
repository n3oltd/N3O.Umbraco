using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Attributes;

public class HorizontalAlignmentAttribute : FormattingAttribute {
    public HorizontalAlignmentAttribute(HorizontalAlignment horizontalAlignment) {
        HorizontalAlignment = horizontalAlignment;
    }

    public override void ApplyFormatting(ExcelFormatting formatting) {
        formatting.HorizontalAlignment = HorizontalAlignment;
    }
    
    public HorizontalAlignment HorizontalAlignment { get; }
}

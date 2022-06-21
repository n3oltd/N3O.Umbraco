using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Attributes;

public class FontWeightAttribute : FormattingAttribute {
    public FontWeightAttribute(FontWeight fontWeight) {
        FontWeight = fontWeight;
    }

    public override void ApplyFormatting(ExcelFormatting formatting) {
        formatting.FontWeight = FontWeight;
    }
    
    public FontWeight FontWeight { get; }
}

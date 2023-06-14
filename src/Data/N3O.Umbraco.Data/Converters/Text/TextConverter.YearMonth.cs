using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Converters;

public class YearMonthTextConverter : ITextConverter<YearMonth?> {
    public string ToInvariantText(YearMonth? value) {
        return value.IfNotNull(x => YearMonthPattern.Iso.Format(x));
    }

    public string ToText(IFormatter formatter, YearMonth? value) {
        return ToInvariantText(value);
    }
}

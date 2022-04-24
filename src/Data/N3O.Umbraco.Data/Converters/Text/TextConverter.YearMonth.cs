using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Converters {
    public class YearMonthTextConverter : ITextConverter<YearMonth?> {
        public string Convert(IFormatter formatter, YearMonth? value) {
            if (value != null) {
                return YearMonthPattern.Iso.Format(value.Value);
            } else {
                return null;
            }
        }
    }
}
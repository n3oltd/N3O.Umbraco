using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters {
    public class TimeTextConverter : ITextConverter<LocalTime?> {
        public string Convert(IFormatter formatter, LocalTime? value) {
            var text = formatter.DateTime.FormatTime(value);

            return text;
        }
    }
}
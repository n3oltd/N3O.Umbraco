using N3O.Umbraco.Extensions;
using NodaTime;
using System;

namespace N3O.Umbraco.Localization {
    public partial class DateTimeFormatter {
        public string FormatTime(LocalTime? localTime, TimeFormat timeFormat = null) {
            return Format(localTime, timeFormat, FormatTime);
        }

        public string FormatTime(LocalTime? localTime, string specifier) {
            return Format(localTime, (DateFormat) null, (dt, _) => FormatTime(dt, specifier));
        }

        private string Format<TDateOrTimeFormat>(LocalTime? localTime, TDateOrTimeFormat format, Func<DateTime?, TDateOrTimeFormat, string> formatter) {
            if (localTime == null) {
                return null;
            }

            var dateTime = localTime.Value
                                    .On(DateTime.UtcNow.ToLocalDate())
                                    .ToDateTimeUnspecified();

            return formatter(dateTime, format);
        }
    }
}
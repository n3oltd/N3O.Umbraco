using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Extensions;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing {
    public class TimeParser : DataTypeParser<LocalTime?>, ITimeParser {
        private readonly Timezone _timezone;
        private readonly Lazy<IDateTimeParser> _dateTimeParser;

        public TimeParser(IDateParser dateParser, Timezone timezone) {
            _timezone = timezone;
            _dateTimeParser = new Lazy<IDateTimeParser>(() => new DateTimeParser(dateParser, timezone));
        }
        
        public override bool CanParse(DataType dataType) {
            return dataType == OurDataTypes.Time;
        }

        protected override Models.ParseResult<LocalTime?> TryParse(string text, Type targetType) {
            LocalTime? value = null;
            
            if (text.HasValue()) {
                text = text.Trim();
                
                value = ParseTimeText(text);
                value ??= ParseUnixTimestamp(text);
                value ??= ParseDateTimeText(text);

                if (value == null) {
                    return ParseResult.Fail<LocalTime?>();
                }
            }

            return ParseResult.Success(value);
        }

        private LocalTime? ParseTimeText(string text) {
            foreach (var pattern in Patterns) {
                var nodaResult = pattern.Parse(text);

                if (nodaResult.Success) {
                    return nodaResult.Value;
                }
            }

            return null;
        }
        
        private LocalTime? ParseUnixTimestamp(string text) {
            text = text.Trim();

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestamp)) {
                return ParseUnixTimestamp(timestamp);
            }
            
            return null;
        }
        
        private LocalTime? ParseUnixTimestamp(int timestamp) {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            var instant = Instant.FromDateTimeOffset(dateTimeOffset);
            
            return instant.InZone(_timezone.Zone).TimeOfDay;
        }

        private LocalTime? ParseDateTimeText(string text) {
            var parseResult = _dateTimeParser.Value.Parse(text, OurDataTypes.DateTime.GetClrType());

            return parseResult.Value?.TimeOfDay;
        }

        protected override IEnumerable<JTokenType> TokenTypes {
            get {
                yield return JTokenType.Date;
                yield return JTokenType.Integer;
            }
        }

        protected override Models.ParseResult<LocalTime?> TryParseToken(JToken token, Type targetType) {
            LocalTime? localTime;

            if (token.Type == JTokenType.Date) {
                var dateTime = (DateTime?) token;

                if (dateTime?.Kind == DateTimeKind.Utc) {
                    localTime = dateTime.Value.InTimezone(_timezone).LocalDateTime.TimeOfDay;
                } else {
                    localTime = dateTime?.ToLocalDateTime().TimeOfDay;
                }
            }else if (token.Type == JTokenType.Integer) {
                var timestamp = (int?) token;
                localTime = timestamp == null ? null : ParseUnixTimestamp(timestamp.Value);
            } else {
                throw UnrecognisedValueException.For(token.Type);
            }

            return ParseResult.Success(localTime);
        }

        public static readonly LocalTimePattern[] Patterns = {
            LocalTimePattern.Create(LocalTimePattern.ExtendedIso.PatternText, CultureInfo.InvariantCulture),
            LocalTimePattern.Create("HH':'mm':'ss", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("H':'m':'s", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("HH':'mm", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("H':'m", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("hh':'mm':'ss", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("h':'m':'s", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("hh':'mm", CultureInfo.InvariantCulture),
            LocalTimePattern.Create("h':'m", CultureInfo.InvariantCulture),
        };
    }
}
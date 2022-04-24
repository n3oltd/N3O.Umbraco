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

namespace N3O.Umbraco.Data.Parsing {
    public class DateTimeParser : DataTypeParser<LocalDateTime?>, IDateTimeParser {
        private readonly Timezone _timezone;
        private readonly List<LocalDateTimePattern> _patterns = new();

        public DateTimeParser(IDateParser dateParser, ITimeParser timeParser, Timezone timezone) {
            _timezone = timezone;
            
            foreach (var datePattern in dateParser.Patterns) {
                foreach (var timePattern in timeParser.Patterns) {
                    AddPattern($"ld<{datePattern.PatternText}> lt<{timePattern.PatternText}>");
                }
            }
        }
        
        public override bool CanParse(DataType dataType) {
            return dataType == DataTypes.DateTime;
        }

        protected override Models.ParseResult<LocalDateTime?> TryParse(string text, Type targetType) {
            LocalDateTime? value = null;
            
            if (text.HasValue()) {
                text = text.Trim();

                foreach (var pattern in _patterns) {
                    var nodaResult = pattern.Parse(text);

                    if (nodaResult.Success) {
                        value = nodaResult.Value;
                        
                        break;
                    }
                }
                
                value ??= ParseUnixTimestamp(text);

                if (value == null) {
                    return ParseResult.Fail<LocalDateTime?>();
                }
            }

            if (_timezone != Timezones.Utc) {
                var zonedDateTime = new ZonedDateTime(value.Value, _timezone.Zone, _timezone.UtcOffset);
                
                value = zonedDateTime.WithZone(Timezones.Utc.Zone).LocalDateTime;
            }

            return ParseResult.Success(value);
        }
        
        protected override IEnumerable<JTokenType> TokenTypes {
            get {
                yield return JTokenType.Date;
                yield return JTokenType.Integer;
            }
        }

        protected override Models.ParseResult<LocalDateTime?> TryParseToken(JToken token, Type targetType) {
            LocalDateTime? localDateTime;
            
            if (token.Type == JTokenType.Date) {
                var dateTime = (DateTime?) token;
                localDateTime = dateTime?.ToLocalDateTime();
            } else if (token.Type == JTokenType.Integer) {
                var timestamp = (int?) token;
                localDateTime = timestamp == null ? null : ParseUnixTimestamp(timestamp.Value);
            } else {
                throw UnrecognisedValueException.For(token.Type);
            }
            
            return ParseResult.Success(localDateTime);
        }

        private void AddPattern(string patternText) {
            var pattern = LocalDateTimePattern.Create(patternText, CultureInfo.InvariantCulture);

            _patterns.Add(pattern);
        }
        
        private LocalDateTime? ParseUnixTimestamp(string text) {
            text = text.Trim();

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestamp)) {
                return ParseUnixTimestamp(timestamp);
            }
            
            return null;
        }
        
        private LocalDateTime? ParseUnixTimestamp(int timestamp) {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            var instant = Instant.FromDateTimeOffset(dateTimeOffset);
            
            return instant.InUtc().LocalDateTime;
        }
    }
}
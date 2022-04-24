using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace N3O.Umbraco.Data.Parsing {
    public abstract class DateParser : DataTypeParser<LocalDate?>, IDateParser {
        private readonly Timezone _timezone;
        private readonly List<LocalDatePattern> _patterns = new();
        private IDateTimeParser _dateTimeParser;

        protected DateParser(Timezone timezone) {
            _timezone = timezone;
        }

        public override bool CanParse(DataType dataType) {
            return dataType == DataTypes.Date;
        }

        protected override Models.ParseResult<LocalDate?> TryParse(string text, Type targetType) {
            LocalDate? value = null;
            
            if (text.HasValue()) {
                value = ParseDateText(text);
                value ??= ParseUnixTimestamp(text);
                value ??= ParseDateTimeText(text, targetType);

                if (value == null) {
                    return ParseResult.Fail<LocalDate?>();
                }
            }

            return ParseResult.Success(value);
        }
        
        protected override IEnumerable<JTokenType> TokenTypes {
            get {
                yield return JTokenType.Date;
                yield return JTokenType.Integer;
            }
        }

        protected override Models.ParseResult<LocalDate?> TryParseToken(JToken token, Type targetType) {
            LocalDate? localDate;
            
            if (token.Type == JTokenType.Date) {
                var dateTime = (DateTime?) token;
                localDate = dateTime?.ToLocalDate();
            } else if (token.Type == JTokenType.Integer) {
                var timestamp = (int?) token;
                localDate = timestamp == null ? null : ParseUnixTimestamp(timestamp.Value);
            } else {
                throw UnrecognisedValueException.For(token.Type);
            }
            
            return ParseResult.Success(localDate);
        }
        
        protected void AddPattern(string patternText) {
            var pattern = LocalDatePattern.Create(patternText, CultureInfo.InvariantCulture);

            _patterns.Add(pattern);
        }

        private LocalDate? ParseDateText(string text) {
            text = text.Trim();

            foreach (var pattern in _patterns) {
                var nodaResult = pattern.Parse(text);

                if (nodaResult.Success) {
                    return nodaResult.Value;
                }
            }

            return null;
        }
        
        private LocalDate? ParseUnixTimestamp(string text) {
            text = text.Trim();

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestamp)) {
                return ParseUnixTimestamp(timestamp);
            }
            
            return null;
        }
        
        private LocalDate? ParseUnixTimestamp(int timestamp) {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            var instant = Instant.FromDateTimeOffset(dateTimeOffset);
            
            return instant.InUtc().Date;
        }
        
        private LocalDate? ParseDateTimeText(string text, Type targetType) {
            var parseResult = DateTimeParser.Parse(text, targetType);

            return parseResult.Value?.Date;
        }

        public IReadOnlyList<LocalDatePattern> Patterns => _patterns;

        private IDateTimeParser DateTimeParser {
            get {
                if (_dateTimeParser == null) {
                    _dateTimeParser = new DateTimeParser(this, new TimeParser(), _timezone);
                }

                return _dateTimeParser;
            }
        }
    }
}

using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
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
        private readonly List<LocalTimePattern> _patterns = new();

        public TimeParser() {
            AddPattern(LocalTimePattern.ExtendedIso.PatternText);

            AddPattern("HH:mm:ss");
            AddPattern("H:m:s");
            AddPattern("HH:mm");
            AddPattern("H:m");

            AddPattern("hh:mm:ss");
            AddPattern("h:m:s");
            AddPattern("hh:mm");
            AddPattern("h:m");
        }
        
        public override bool CanParse(DataType dataType) {
            return dataType == OurDataTypes.Time;
        }

        protected override Models.ParseResult<LocalTime?> TryParse(string text, Type targetType) {
            LocalTime? value = null;
            
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
                    return ParseResult.Fail<LocalTime?>();
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

        protected override Models.ParseResult<LocalTime?> TryParseToken(JToken token, Type targetType) {
            LocalTime? localTime;

            if (token.Type == JTokenType.Date) {
                var dateTime = (DateTime?) token;
                localTime = dateTime?.ToLocalDateTime().TimeOfDay;
            }else if (token.Type == JTokenType.Integer) {
                var timestamp = (int?) token;
                localTime = timestamp == null ? null : ParseUnixTimestamp(timestamp.Value);
            } else {
                throw UnrecognisedValueException.For(token.Type);
            }

            return ParseResult.Success(localTime);
        }

        public IReadOnlyList<LocalTimePattern> Patterns => _patterns;

        private void AddPattern(string patternText) {
            var pattern = LocalTimePattern.Create(patternText, CultureInfo.InvariantCulture);

            _patterns.Add(pattern);
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
            
            return instant.InUtc().TimeOfDay;
        }
    }
}
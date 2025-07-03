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

namespace N3O.Umbraco.Data.Parsing;

public class DateTimeParser : DataTypeParser<LocalDateTime?>, IDateTimeParser {
    private readonly Timezone _timezone;
    private readonly List<LocalDateTimePattern> _localPatterns = [];
    private readonly List<OffsetDateTimePattern> _offsetPatterns = [];

    public DateTimeParser(IDateParser dateParser, Timezone timezone) {
        _timezone = timezone;
        
        _offsetPatterns.Add(OffsetDateTimePattern.ExtendedIso);
        _offsetPatterns.Add(OffsetDateTimePattern.GeneralIso);
        _localPatterns.Add(LocalDateTimePattern.ExtendedIso);
        _localPatterns.Add(LocalDateTimePattern.GeneralIso);
        
        foreach (var datePattern in dateParser.Patterns) {
            foreach (var timePattern in TimeParser.Patterns) {
                AddOffsetPattern(datePattern.PatternText, timePattern.PatternText);
                AddLocalPattern(datePattern.PatternText, timePattern.PatternText);
            }
        }
    }
    
    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.DateTime;
    }

    protected override Models.ParseResult<LocalDateTime?> TryParse(string text, Type targetType) {
        LocalDateTime? value = null;
        
        if (text.HasValue()) {
            text = text.Trim();

            foreach (var pattern in _offsetPatterns) {
                var nodaResult = pattern.Parse(text);

                if (nodaResult.Success) {
                    value = nodaResult.Value.InZone(_timezone.Zone).LocalDateTime;
                    
                    break;
                }
            }

            if (value == null) {
                foreach (var pattern in _localPatterns) {
                    var nodaResult = pattern.Parse(text);

                    if (nodaResult.Success) {
                        value = nodaResult.Value;
                    
                        break;
                    }
                }
            }
            
            value ??= ParseUnixTimestamp(text);

            if (value == null) {
                return ParseResult.Fail<LocalDateTime?>();
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

    protected override Models.ParseResult<LocalDateTime?> TryParseToken(JToken token, Type targetType) {
        LocalDateTime? localDateTime;
        
        if (token.Type == JTokenType.Date) {
            var dateTime = (DateTime?) token;
            
            if (dateTime?.Kind == DateTimeKind.Utc) {
                localDateTime = dateTime.Value.InTimezone(_timezone).LocalDateTime;
            } else {
                localDateTime = dateTime?.ToLocalDateTime();
            }
        } else if (token.Type == JTokenType.Integer) {
            var timestamp = (int?) token;
            localDateTime = timestamp == null ? null : ParseUnixTimestamp(timestamp.Value);
        } else {
            throw UnrecognisedValueException.For(token.Type);
        }
        
        return ParseResult.Success(localDateTime);
    }
    
    private void AddLocalPattern(string datePatternText, string timePatternText) {
        var patterns = new[] {
            LocalDateTimePattern.Create($"ld<{datePatternText}>'T'lt<{timePatternText}>", CultureInfo.InvariantCulture),
            LocalDateTimePattern.Create($"ld<{datePatternText}> lt<{timePatternText}>", CultureInfo.InvariantCulture)
        };

        _localPatterns.AddRange(patterns);
    }
    
    private void AddOffsetPattern(string datePatternText, string timePatternText) {
        var patterns = new[] {
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}>'T'lt<{timePatternText}> '('o<g>')'"),
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}> lt<{timePatternText}> '('o<g>')'"),
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}>'T'lt<{timePatternText}> o<g>"),
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}> lt<{timePatternText}> o<g>"),
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}>'T'lt<{timePatternText}>o<g>"),
            OffsetDateTimePattern.CreateWithInvariantCulture($"ld<{datePatternText}> lt<{timePatternText}>o<g>")
        };

        _offsetPatterns.AddRange(patterns);
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
        
        return instant.InZone(_timezone.Zone).LocalDateTime;
    }
}

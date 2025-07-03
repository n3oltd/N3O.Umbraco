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
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public abstract class DateParser : DataTypeParser<LocalDate?>, IDateParser {
    private readonly List<LocalDatePattern> _patterns = [];
    private readonly Lazy<IDateTimeParser> _dateTimeParser;
    private readonly Timezone _timezone;

    protected DateParser(Timezone timezone) {
        _timezone = timezone;
        _dateTimeParser = new Lazy<IDateTimeParser>(() => new DateTimeParser(this, timezone));
        
        AddPattern(LocalDatePattern.Iso.PatternText);
    }

    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.Date;
    }

    protected override Models.ParseResult<LocalDate?> TryParse(string text, Type targetType) {
        LocalDate? value = null;
        
        if (text.HasValue()) {
            text = text.Trim();
            
            value = ParseDateText(text);
            value ??= ParseUnixTimestamp(text);
            value ??= ParseDateTimeText(text);

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

            if (dateTime?.Kind == DateTimeKind.Utc) {
                localDate = dateTime.Value.InTimezone(_timezone).LocalDateTime.Date;
            } else {
                localDate = dateTime?.ToLocalDate();
            }
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
        foreach (var pattern in _patterns) {
            var nodaResult = pattern.Parse(text);

            if (nodaResult.Success) {
                return nodaResult.Value;
            }
        }

        return null;
    }
    
    private LocalDate? ParseUnixTimestamp(string text) {
        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timestamp)) {
            return ParseUnixTimestamp(timestamp);
        }
        
        return null;
    }
    
    private LocalDate? ParseUnixTimestamp(int timestamp) {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        var instant = Instant.FromDateTimeOffset(dateTimeOffset);

        return instant.InZone(_timezone.Zone).Date;
    }
    
    private LocalDate? ParseDateTimeText(string text) {
        var parseResult = _dateTimeParser.Value.Parse(text, OurDataTypes.DateTime.GetClrType());

        return parseResult.Value?.Date;
    }

    public IReadOnlyList<LocalDatePattern> Patterns => _patterns;
}

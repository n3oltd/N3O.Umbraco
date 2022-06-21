using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using DecimalSeparatorCharacters = N3O.Umbraco.Data.DataConstants.DecimalSeparators;

namespace N3O.Umbraco.Data.Parsing;

public abstract class NumberParser<T> : DataTypeParser<T?> where T : struct {
    private readonly DataType _dataType;
    private readonly DecimalSeparator _decimalSeparator;

    protected NumberParser(DataType dataType,
                           DecimalSeparator decimalSeparator,
                           IEnumerable<JTokenType> tokenTypes) {
        _dataType = dataType;
        _decimalSeparator = decimalSeparator;

        TokenTypes = tokenTypes;
    }

    public override bool CanParse(DataType dataType) {
        return _dataType == dataType;
    }

    protected override ParseResult<T?> TryParse(string text, Type targetType) {
        try {
            text = Normalize(text);

            var value = text.HasValue() ? decimal.Parse(text, CultureInfo.InvariantCulture) : (decimal?) null;
            var convertedValue = ConvertDecimal(value);

            return ParseResult.Success(convertedValue);
        } catch {
            return ParseResult.Fail<T?>();
        }
    }

    protected abstract T? ConvertDecimal(decimal? value);

    protected override IEnumerable<JTokenType> TokenTypes { get; }

    protected override ParseResult<T?> TryParseToken(JToken token, Type targetType) {
        return ParseResult.Success(token.Value<T?>());
    }

    private string Normalize(string text) {
        if (text == null) {
            return null;
        }

        var charactersToStripRegex = $@"[^0-9{_decimalSeparator.SeparatorRegex}\-]";

        text = Regex.Replace(text, charactersToStripRegex, string.Empty);
        text = Regex.Replace(text, $"[{_decimalSeparator.SeparatorRegex}]", DecimalSeparatorCharacters.Point);
        if (text != "0") {
            text = text.TrimStart('0');
        }

        return text;
    }
}

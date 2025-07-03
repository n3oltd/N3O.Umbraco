using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public abstract class YearMonthParser : DataTypeParser<YearMonth?>, IYearMonthParser {
    private readonly List<YearMonthPattern> _patterns = [];

    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.YearMonth;
    }

    protected override Models.ParseResult<YearMonth?> TryParse(string text, Type targetType) {
        YearMonth? value = null;

        if (text.HasValue()) {
            text = text.Trim();

            foreach (var pattern in _patterns) {
                var nodaResult = pattern.Parse(text);

                if (nodaResult.Success) {
                    value = nodaResult.Value;

                    break;
                }
            }

            if (value == null) {
                return ParseResult.Fail<YearMonth?>();
            }
        }

        return ParseResult.Success(value);
    }

    protected void AddPattern(string patternText) {
        var pattern = YearMonthPattern.Create(patternText, CultureInfo.InvariantCulture);

        _patterns.Add(pattern);
    }
}

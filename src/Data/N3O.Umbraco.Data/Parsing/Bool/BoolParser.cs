using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public class BoolParser : DataTypeParser<bool?>, IBoolParser {
    private readonly ILookupParser _lookupParser;

    public BoolParser(ILookupParser lookupParser) {
        _lookupParser = lookupParser;
    }

    public override bool CanParse(DataType dataType) {
        return dataType == OurDataTypes.Bool;
    }

    protected override ParseResult<bool?> TryParse(string text, Type targetType) {
        var parseResult = _lookupParser.Parse<BooleanValue>(text);

        if (parseResult.Success) {
            return ParseResult.Success(parseResult.Value?.ClrValue);
        } else {
            return ParseResult.Fail<bool?>();
        }
    }

    protected override IEnumerable<JTokenType> TokenTypes => JTokenType.Boolean.Yield();

    protected override ParseResult<bool?> TryParseToken(JToken token, Type targetType) {
        return ParseResult.Success((bool?) token);
    }
}

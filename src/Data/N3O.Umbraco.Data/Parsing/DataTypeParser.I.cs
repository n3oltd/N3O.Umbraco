using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Data.Parsing;

public interface IDataTypeParser {
    bool CanParse(DataType dataType);
    ParseResult<object> ParseToObject(string text, Type targetType);
    ParseResult<object> ParseToObject(JToken token, Type targetType);
}

public interface IDataTypeParser<T> : IDataTypeParser {
    ParseResult<T> Parse(string text, Type targetType);
    ParseResult<T> Parse(JToken token, Type targetType);
}

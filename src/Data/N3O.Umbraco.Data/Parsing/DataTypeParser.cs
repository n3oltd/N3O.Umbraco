using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Parsing {
    public abstract class DataTypeParser<T> : IDataTypeParser<T> {
        public ParseResult<object> ParseToObject(string text, Type targetType) {
            var result = Parse(text, targetType);

            return ConvertResult(result);
        }

        public ParseResult<object> ParseToObject(JToken token, Type targetType) {
            var result = Parse(token, targetType);

            return ConvertResult(result);
        }

        public abstract bool CanParse(DataType dataType);

        public virtual ParseResult<T> Parse(string text, Type targetType) {
            var parseResult = TryParse(text, targetType);

            return parseResult;
        }

        public ParseResult<T> Parse(JToken token, Type targetType) {
            ParseResult<T> parseResult;

            if (token.Type == JTokenType.String) {
                var text = (string) token;

                parseResult = Parse(text, targetType);
            } else if (TokenTypes.Contains(token.Type)) {
                parseResult = TryParseToken(token, targetType);
            } else {
                parseResult = ParseResult.Fail<T>();
            }

            return parseResult;
        }

        protected virtual ParseResult<T> TryParse(string text, Type targetType) {
            throw new NotImplementedException();
        }

        protected virtual ParseResult<T> TryParseToken(JToken token, Type targetType) {
            throw new NotImplementedException();
        }

        private ParseResult<object> ConvertResult(ParseResult<T> result) {
            if (result.Success) {
                return ParseResult.Success<object>(result.Value);
            } else {
                return ParseResult.Fail<object>();
            }
        }

        protected virtual IEnumerable<JTokenType> TokenTypes => Enumerable.Empty<JTokenType>();
    }
}
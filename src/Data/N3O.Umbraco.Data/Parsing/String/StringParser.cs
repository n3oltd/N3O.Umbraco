using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing {
    public class StringParser : DataTypeParser<string>, IStringParser {
        public override bool CanParse(DataType dataType) {
            return dataType == OurDataTypes.String;
        }

        protected override ParseResult<string> TryParse(string text, Type targetType) {
            var result = text?.Trim();

            if (string.IsNullOrWhiteSpace(result)) {
                result = null;
            }

            return ParseResult.Success(result);
        }

        protected override ParseResult<string> TryParseToken(JToken token, Type targetType) {
            return ParseResult.Success(token.ToString());
        }

        protected override IEnumerable<JTokenType> TokenTypes => Enum.GetValues<JTokenType>();
    }
}
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Umbraco.Extensions;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing {
    public class GuidParser : DataTypeParser<Guid?>, IGuidParser {
        public override bool CanParse(DataType dataType) {
            return dataType == OurDataTypes.Guid;
        }

        protected override ParseResult<Guid?> TryParse(string text, Type targetType) {
            if (Guid.TryParse(text?.Trim(), out var guid)) {
                return ParseResult.Success<Guid?>(guid);
            } else {
                return ParseResult.Fail<Guid?>();
            }
        }

        protected override IEnumerable<JTokenType> TokenTypes => JTokenType.Guid.Yield();

        protected override ParseResult<Guid?> TryParseToken(JToken token, Type targetType) {
            var guid = (Guid?) token;

            return ParseResult.Success(guid);
        }
    }
}
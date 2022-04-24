using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Data.Parsing {
    public class MoneyParser : DataTypeParser<Money>, IMoneyParser {
        private readonly IDecimalParser _decimalParser;

        public MoneyParser(IDecimalParser decimalParser) {
            _decimalParser = decimalParser;
        }

        public override bool CanParse(DataType dataType) {
            return dataType == DataTypes.Money;
        }

        protected override ParseResult<Money> TryParse(string text, Type targetType) {
            throw new NotImplementedException();
        }

        public ParseResult<Money> Parse(string text, Type targetType, Currency currency) {
            var parsedAmount = _decimalParser.Parse(text, targetType);

            if (parsedAmount.Success) {
                Money value = null;

                if (parsedAmount.Value.HasValue()) {
                    value = new Money(parsedAmount.Value.GetValueOrThrow(), currency);
                }

                return ParseResult.Success(value);
            } else {
                return ParseResult.Fail<Money>();
            }
        }
    }
}
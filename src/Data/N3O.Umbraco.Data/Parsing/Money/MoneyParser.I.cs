using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;
using System;

namespace N3O.Umbraco.Data.Parsing {
    public interface IMoneyParser : IDataTypeParser<Money> {
        ParseResult<Money> Parse(string text, Type targetType, Currency currency);
    }
}
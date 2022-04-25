using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using Newtonsoft.Json.Linq;
using System;

namespace N3O.Umbraco.Data.Parsing {
    public interface IParser {
        IBoolParser Bool { get; }
        IContentParser Content { get; }
        IDateParser Date { get; }
        IDateTimeParser DateTime { get; }
        IDecimalParser Decimal { get; }
        IGuidParser Guid { get; }
        IIntegerParser Integer { get; }
        ILookupParser Lookup { get; }
        IMoneyParser Money { get; }
        IPublishedContentParser PublishedContent { get; }
        IReferenceParser Reference { get; }
        IStringParser String { get; }
        ITimeParser Time { get; }
        IYearMonthParser YearMonth { get; }

        ParseResult<object> Parse(string text, DataType dataType, Type targetType);
        ParseResult<object> Parse(JToken token, DataType dataType, Type targetType);
    }
}

using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Parsing {
    public class Parser : IParser {
        private readonly IReadOnlyList<IDataTypeParser> _allParsers;

        public Parser(IBoolParser boolParser,
                      IContentParser contentParser,
                      IDateParser dateParser,
                      IDateTimeParser dateTimeParser,
                      IDecimalParser decimalParser,
                      IGuidParser guidParser,
                      IIntegerParser integerParser,
                      ILookupParser lookupParser,
                      IMoneyParser moneyParser,
                      IPublishedContentParser publishedContentParser,
                      IReferenceParser referenceParser,
                      IStringParser textParser,
                      ITimeParser timeParser,
                      IYearMonthParser yearMonth) {
            Bool = boolParser;
            Content = contentParser;
            Date = dateParser;
            DateTime = dateTimeParser;
            Decimal = decimalParser;
            Guid = guidParser;
            Integer = integerParser;
            Lookup = lookupParser;
            Money = moneyParser;
            PublishedContent = publishedContentParser;
            Reference = referenceParser;
            String = textParser;
            Time = timeParser;
            YearMonth = yearMonth;

            _allParsers = new IDataTypeParser[] {
                Bool,
                Content,
                Date,
                DateTime,
                Decimal,
                Guid,
                Integer,
                Lookup,
                Money,
                PublishedContent,
                Reference,
                String, Time
            };
        }

        public IBoolParser Bool { get; }
        public IContentParser Content { get; }
        public IDateParser Date { get; }
        public IDateTimeParser DateTime { get; }
        public IDecimalParser Decimal { get; }
        public IGuidParser Guid { get; }
        public IIntegerParser Integer { get; }
        public ILookupParser Lookup { get; }
        public IMoneyParser Money { get; }
        public IPublishedContentParser PublishedContent { get; }
        public IReferenceParser Reference { get; }
        public IStringParser String { get; }
        public ITimeParser Time { get; }
        public IYearMonthParser YearMonth { get; }

        public ParseResult<object> Parse(string text, DataType dataType, Type targetType) {
            var parser = GetParser(dataType);

            return parser.ParseToObject(text, targetType);
        }

        public ParseResult<object> Parse(JToken token, DataType dataType, Type targetType) {
            var parser = GetParser(dataType);

            return parser.ParseToObject(token, targetType);
        }

        private IDataTypeParser GetParser(DataType dataType) {
            var parser = _allParsers.SingleOrDefault(x => x.CanParse(dataType));

            if (parser == null) {
                throw new Exception($"No {nameof(IDataTypeParser)} found for data type {dataType.Id.Quote()}");
            }

            return parser;
        }
    }
}
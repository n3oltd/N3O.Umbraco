using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Parsing {
    public class ParserFactory : IParserFactory {
        private readonly IBoolParser _boolParser;
        private readonly IContentParser _contentParser;
        private readonly IDateParserFactory _dateParserFactory;
        private readonly IDecimalParserFactory _decimalParserFactory;
        private readonly IGuidParser _guidParser;
        private readonly IIntegerParserFactory _integerParserFactory;
        private readonly ILookupParser _lookupParser;
        private readonly IPublishedContentParser _publishedContentParser;
        private readonly IReferenceParser _referenceParser;
        private readonly IStringParser _stringParser;
        private readonly ITimeParser _timeParser;
        private readonly IYearMonthParserFactory _yearMonthParserFactory;

        public ParserFactory(IBoolParser boolParser,
                             IContentParser contentParser,
                             IDateParserFactory dateParserFactory,
                             IDecimalParserFactory decimalParserFactory,
                             IGuidParser guidParser,
                             IIntegerParserFactory integerParserFactory,
                             ILookupParser lookupParser,
                             IPublishedContentParser publishedContentParser,
                             IReferenceParser referenceParser,
                             IStringParser stringParser,
                             ITimeParser timeParser,
                             IYearMonthParserFactory yearMonthParserFactory) {
            _boolParser = boolParser;
            _contentParser = contentParser;
            _yearMonthParserFactory = yearMonthParserFactory;
            _dateParserFactory = dateParserFactory;
            _decimalParserFactory = decimalParserFactory;
            _guidParser = guidParser;
            _integerParserFactory = integerParserFactory;
            _lookupParser = lookupParser;
            _publishedContentParser = publishedContentParser;
            _referenceParser = referenceParser;
            _stringParser = stringParser;
            _timeParser = timeParser;
        }

        public IParser GetParser(DatePattern datePattern,
                                 DecimalSeparator decimalSeparator,
                                 IEnumerable<IBlobResolver> blobResolvers,
                                 Timezone timezone = null) {
            timezone ??= Timezones.Utc;

            var blobParser = new BlobParser(blobResolvers);
            var dateParser = _dateParserFactory.Create(datePattern, timezone);
            var dateTimeParser = new DateTimeParser(dateParser, _timeParser, timezone);
            var decimalParser = _decimalParserFactory.Create(decimalSeparator);
            var integerParser = _integerParserFactory.Create(decimalSeparator);
            var moneyParser = new MoneyParser(decimalParser);
            var yearMonthParser = _yearMonthParserFactory.Create(datePattern);

            var parser = new Parser(blobParser,
                                    _boolParser,
                                    _contentParser,
                                    dateParser,
                                    dateTimeParser,
                                    decimalParser,
                                    _guidParser,
                                    integerParser,
                                    _lookupParser,
                                    moneyParser,
                                    _publishedContentParser,
                                    _referenceParser,
                                    _stringParser,
                                    _timeParser,
                                    yearMonthParser);

            return parser;
        }
    }
}
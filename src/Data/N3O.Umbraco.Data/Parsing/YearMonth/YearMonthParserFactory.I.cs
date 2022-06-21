using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Parsing;

public interface IYearMonthParserFactory {
    IYearMonthParser Create(DatePattern pattern);
}

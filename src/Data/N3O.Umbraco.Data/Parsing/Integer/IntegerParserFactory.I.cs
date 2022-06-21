using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Parsing;

public interface IIntegerParserFactory {
    IIntegerParser Create(DecimalSeparator decimalSeparator);
}

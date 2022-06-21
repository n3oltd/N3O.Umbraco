using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Data.Parsing;

public class IntegerParserFactory : IIntegerParserFactory {
    public IIntegerParser Create(DecimalSeparator decimalSeparator) {
        if (decimalSeparator == DecimalSeparators.Comma) {
            return new CommaIntegerParser();
        } else if (decimalSeparator == DecimalSeparators.Point) {
            return new PointIntegerParser();
        } else {
            throw UnrecognisedValueException.For(decimalSeparator);
        }
    }
}

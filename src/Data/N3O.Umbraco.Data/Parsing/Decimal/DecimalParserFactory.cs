using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Data.Parsing;

public class DecimalParserFactory : IDecimalParserFactory {
    public IDecimalParser Create(DecimalSeparator decimalSeparator) {
        if (decimalSeparator == DecimalSeparators.Comma) {
            return new CommaDecimalParser();
        } else if (decimalSeparator == DecimalSeparators.Point) {
            return new PointDecimalParser();
        } else {
            throw UnrecognisedValueException.For(decimalSeparator);
        }
    }
}

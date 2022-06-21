using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Parsing;

public class CommaDecimalParser : NumberParser<decimal>, IDecimalParser {
    public CommaDecimalParser()
        : base(OurDataTypes.Decimal, DecimalSeparators.Comma, new[] { JTokenType.Float, JTokenType.Integer }) { }
    
    protected override decimal? ConvertDecimal(decimal? value) {
        return value;
    }
}

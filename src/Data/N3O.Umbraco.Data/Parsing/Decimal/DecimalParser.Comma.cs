using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Data.Parsing {
    public class CommaDecimalParser : NumberParser<decimal>, IDecimalParser {
        public CommaDecimalParser()
            : base(DataTypes.Decimal, DecimalSeparators.Comma, new[] { JTokenType.Float, JTokenType.Integer }) { }
        
        protected override decimal? ConvertDecimal(decimal? value) {
            return value;
        }
    }
}
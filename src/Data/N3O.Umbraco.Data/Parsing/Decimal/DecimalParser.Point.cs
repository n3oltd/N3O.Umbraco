using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Data.Parsing {
    public class PointDecimalParser : NumberParser<decimal>, IDecimalParser {
        public PointDecimalParser()
            : base(DataTypes.Decimal, DecimalSeparators.Point, new[] { JTokenType.Float, JTokenType.Integer }) { }
        
        protected override decimal? ConvertDecimal(decimal? value) {
            return value;
        }
    }
}
using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Parsing {
    public class PointIntegerParser : NumberParser<long>, IIntegerParser {
        public PointIntegerParser()
            : base(DataTypes.Integer, DecimalSeparators.Point, JTokenType.Integer.Yield()) { }

        protected override long? ConvertDecimal(decimal? value) {
            return (long?) value;
        }
    }
}
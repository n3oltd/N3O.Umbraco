using N3O.Umbraco.Data.Lookups;
using Newtonsoft.Json.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Parsing {
    public class CommaIntegerParser : NumberParser<long>, IIntegerParser {
        public CommaIntegerParser()
            : base(DataTypes.Integer, DecimalSeparators.Comma, JTokenType.Integer.Yield()) { }

        protected override long? ConvertDecimal(decimal? value) {
            return (long?) value;
        }
    }
}
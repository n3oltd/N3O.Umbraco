using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public interface IParserFactory {
        IParser GetParser(DatePattern datePattern, DecimalSeparator decimalSeparator, Timezone timezone = null);
    }
}
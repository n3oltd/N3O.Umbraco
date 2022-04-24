using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Parsing {
    public interface IDateParserFactory {
        IDateParser Create(DatePattern pattern, Timezone timezone);
    }
}
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Parsing {
    public interface IParserFactory {
        IParser GetParser(DatePattern datePattern,
                          DecimalSeparator decimalSeparator,
                          IEnumerable<IBlobResolver> blobResolvers,
                          Timezone timezone = null);
    }
}
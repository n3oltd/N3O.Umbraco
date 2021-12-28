using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Localization {
    public class TextResource {
        public string Source { get; set; }
        public string Custom { get; set; }

        public string Value => Custom.Or(Source);
    }
}

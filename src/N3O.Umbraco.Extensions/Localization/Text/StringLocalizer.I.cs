using System.Collections.Generic;

namespace N3O.Umbraco.Localization {
    public interface IStringLocalizer {
        void Flush(IEnumerable<string> aliases);
        string Get(string folder, string name, string text);
    }
}

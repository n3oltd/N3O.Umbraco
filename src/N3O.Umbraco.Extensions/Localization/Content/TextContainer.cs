using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Localization {
    public class TextContainer : UmbracoContent<TextContainer> {
        public IEnumerable<TextResource> Resources => GetValue(x => x.Resources);
    }
}
